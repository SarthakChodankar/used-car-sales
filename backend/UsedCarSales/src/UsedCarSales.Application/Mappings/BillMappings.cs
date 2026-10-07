using UsedCarSales.Application.DTOs.Billing;
using UsedCarSales.Domain.Entities;

namespace UsedCarSales.Application.Mappings;

public static class BillMappings
{
    public static BillDto ToDto(this Bill b) => new()
    {
        BillId = b.BillId,
        InvoiceNumber = b.InvoiceNumber,
        BillDate = b.BillDate,
        SalePrice = b.SalePrice,
        TaxPercent = b.TaxPercent,
        TaxAmount = b.TaxAmount,
        TotalAmount = b.TotalAmount,
        PaymentMethod = b.PaymentMethod,
        Car = new BillCarDto
        {
            CarId = b.Car.CarId,
            Vin = b.Car.Vin,
            Make = b.Car.Make,
            Model = b.Car.Model,
            ManufactureYear = b.Car.ManufactureYear
        },
        Customer = new BillCustomerDto
        {
            CustomerId = b.Customer.CustomerId,
            FullName = b.Customer.FullName,
            Email = b.Customer.Email,
            Phone = b.Customer.Phone,
            Address = b.Customer.Address
        }
    };
}