using UsedCarSales.Application.DTOs.Customers;
using UsedCarSales.Domain.Entities;

namespace UsedCarSales.Application.Mappings;

public static class CustomerMappings
{
    public static CustomerDto ToDto(this Customer c) => new()
    {
        CustomerId = c.CustomerId,
        FullName = c.FullName,
        Email = c.Email,
        Phone = c.Phone,
        Address = c.Address,
        CreatedAt = c.CreatedAt
    };

    public static void ApplyTo(this CustomerSaveRequest request, Customer c)
    {
        c.FullName = request.FullName.Trim();
        c.Email = request.Email.Trim().ToLowerInvariant();
        c.Phone = request.Phone.Trim();
        c.Address = request.Address?.Trim();
    }
}