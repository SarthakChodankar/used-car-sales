using UsedCarSales.Application.Common;
using UsedCarSales.Application.DTOs.Billing;

namespace UsedCarSales.Application.Interfaces;

public interface IBillingService
{
    Task<BillDto> CreateSaleAsync(SaleRequest request, CancellationToken ct = default);
    Task<PagedResult<BillDto>> GetAllAsync(BillQueryParams query, CancellationToken ct = default);
    Task<BillDto> GetByIdAsync(int billId, CancellationToken ct = default);
}