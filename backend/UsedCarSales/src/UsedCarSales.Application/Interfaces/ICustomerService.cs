using UsedCarSales.Application.Common;
using UsedCarSales.Application.DTOs.Customers;

namespace UsedCarSales.Application.Interfaces;

public interface ICustomerService
{
    Task<PagedResult<CustomerDto>> GetAllAsync(CustomerQueryParams query, CancellationToken ct = default);
    Task<CustomerDto> GetByIdAsync(int customerId, CancellationToken ct = default);
    Task<CustomerDto> CreateAsync(CustomerSaveRequest request, CancellationToken ct = default);
    Task UpdateAsync(int customerId, CustomerSaveRequest request, CancellationToken ct = default);
    Task DeleteAsync(int customerId, CancellationToken ct = default);
}