using UsedCarSales.Application.Common;
using UsedCarSales.Application.DTOs.Billing;
using UsedCarSales.Application.Interfaces;
using UsedCarSales.Application.Mappings;
using UsedCarSales.Domain.Constants;
using UsedCarSales.Domain.Entities;
using UsedCarSales.Domain.Exceptions;

namespace UsedCarSales.Application.Services;

public class BillingService : IBillingService
{
    private readonly IBillRepository _bills;
    private readonly ICarRepository _cars;
    private readonly ICustomerRepository _customers;

    public BillingService(IBillRepository bills, ICarRepository cars, ICustomerRepository customers)
    {
        _bills = bills;
        _cars = cars;
        _customers = customers;
    }

    public async Task<BillDto> CreateSaleAsync(SaleRequest request, CancellationToken ct = default)
    {
        var car = await _cars.GetByIdAsync(request.CarId, ct)
                  ?? throw new NotFoundException("Car", request.CarId);

        if (car.Status != CarStatus.Available)
            throw new BusinessRuleException($"This car is {car.Status} and cannot be sold.");

        var customer = await _customers.GetByIdAsync(request.CustomerId, ct)
                       ?? throw new NotFoundException("Customer", request.CustomerId);

        var taxAmount = Math.Round(
            request.SalePrice * BillingRules.TaxPercent / 100m, 2, MidpointRounding.AwayFromZero);

        var bill = new Bill
        {
            InvoiceNumber = GenerateInvoiceNumber(),
            Car = car,
            Customer = customer,
            SalePrice = request.SalePrice,
            TaxPercent = BillingRules.TaxPercent,
            TaxAmount = taxAmount,
            TotalAmount = request.SalePrice + taxAmount,
            PaymentMethod = request.PaymentMethod,
            BillDate = DateTime.UtcNow
        };

        car.Status = CarStatus.Sold;
        car.UpdatedAt = DateTime.UtcNow;

        // One SaveChanges = one database transaction: bill is created and car is marked Sold together.
        await _bills.AddAsync(bill, ct);
        await _bills.SaveChangesAsync(ct);

        return bill.ToDto();
    }

    public async Task<PagedResult<BillDto>> GetAllAsync(BillQueryParams query, CancellationToken ct = default)
    {
        var (items, total) = await _bills.GetPagedAsync(query, ct);

        return new PagedResult<BillDto>
        {
            Items = items.Select(b => b.ToDto()).ToList(),
            TotalCount = total,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }

    public async Task<BillDto> GetByIdAsync(int billId, CancellationToken ct = default)
    {
        var bill = await _bills.GetByIdAsync(billId, ct)
                   ?? throw new NotFoundException("Bill", billId);
        return bill.ToDto();
    }

    private static string GenerateInvoiceNumber() =>
        $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";
}