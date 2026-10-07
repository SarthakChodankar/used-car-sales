using UsedCarSales.Application.Common;
using UsedCarSales.Application.DTOs.Customers;
using UsedCarSales.Application.Interfaces;
using UsedCarSales.Application.Mappings;
using UsedCarSales.Domain.Entities;
using UsedCarSales.Domain.Exceptions;

namespace UsedCarSales.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repo;

    public CustomerService(ICustomerRepository repo) => _repo = repo;

    public async Task<PagedResult<CustomerDto>> GetAllAsync(CustomerQueryParams query, CancellationToken ct = default)
    {
        var (items, total) = await _repo.GetPagedAsync(query, ct);

        return new PagedResult<CustomerDto>
        {
            Items = items.Select(c => c.ToDto()).ToList(),
            TotalCount = total,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }

    public async Task<CustomerDto> GetByIdAsync(int customerId, CancellationToken ct = default)
    {
        var customer = await _repo.GetByIdAsync(customerId, ct)
                       ?? throw new NotFoundException("Customer", customerId);
        return customer.ToDto();
    }

    public async Task<CustomerDto> CreateAsync(CustomerSaveRequest request, CancellationToken ct = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await _repo.EmailExistsAsync(email, null, ct))
            throw new BusinessRuleException($"A customer with email '{email}' already exists.");

        var customer = new Customer { CreatedAt = DateTime.UtcNow };
        request.ApplyTo(customer);

        await _repo.AddAsync(customer, ct);
        await _repo.SaveChangesAsync(ct);

        return customer.ToDto();
    }

    public async Task UpdateAsync(int customerId, CustomerSaveRequest request, CancellationToken ct = default)
    {
        var customer = await _repo.GetByIdAsync(customerId, ct)
                       ?? throw new NotFoundException("Customer", customerId);

        var email = request.Email.Trim().ToLowerInvariant();
        if (await _repo.EmailExistsAsync(email, customerId, ct))
            throw new BusinessRuleException($"A customer with email '{email}' already exists.");

        request.ApplyTo(customer);
        await _repo.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int customerId, CancellationToken ct = default)
    {
        var customer = await _repo.GetByIdAsync(customerId, ct)
                       ?? throw new NotFoundException("Customer", customerId);

        if (await _repo.HasBillsAsync(customerId, ct))
            throw new BusinessRuleException("This customer has billing history and cannot be deleted.");

        _repo.Remove(customer);
        await _repo.SaveChangesAsync(ct);
    }
}