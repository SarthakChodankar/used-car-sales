using UsedCarSales.Application.DTOs.Billing;
using UsedCarSales.Domain.Entities;

namespace UsedCarSales.Application.Interfaces;

public interface IBillRepository
{
    Task<(IReadOnlyList<Bill> Items, int TotalCount)> GetPagedAsync(
        BillQueryParams query, CancellationToken ct = default);

    Task<Bill?> GetByIdAsync(int billId, CancellationToken ct = default);

    Task AddAsync(Bill bill, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}