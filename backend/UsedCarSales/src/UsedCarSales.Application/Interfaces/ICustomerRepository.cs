using UsedCarSales.Application.DTOs.Customers;
using UsedCarSales.Domain.Entities;

namespace UsedCarSales.Application.Interfaces;

public interface ICustomerRepository
{
    Task<(IReadOnlyList<Customer> Items, int TotalCount)> GetPagedAsync(
        CustomerQueryParams query, CancellationToken ct = default);

    Task<Customer?> GetByIdAsync(int customerId, CancellationToken ct = default);

    Task<bool> EmailExistsAsync(string email, int? excludeCustomerId, CancellationToken ct = default);

    Task<bool> HasBillsAsync(int customerId, CancellationToken ct = default);

    Task AddAsync(Customer customer, CancellationToken ct = default);

    void Remove(Customer customer);

    Task SaveChangesAsync(CancellationToken ct = default);
}