using Microsoft.EntityFrameworkCore;
using UsedCarSales.Application.DTOs.Billing;
using UsedCarSales.Application.Interfaces;
using UsedCarSales.Domain.Entities;
using UsedCarSales.Infrastructure.Persistence;

namespace UsedCarSales.Infrastructure.Repositories;

public class BillRepository : IBillRepository
{
    private readonly AppDbContext _db;

    public BillRepository(AppDbContext db) => _db = db;

    public async Task<(IReadOnlyList<Bill> Items, int TotalCount)> GetPagedAsync(
        BillQueryParams query, CancellationToken ct = default)
    {
        var bills = _db.Bills.AsNoTracking()
            .Include(b => b.Car)
            .Include(b => b.Customer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim();
            bills = bills.Where(b => b.InvoiceNumber.Contains(term)
                                  || b.Customer.FullName.Contains(term));
        }

        var total = await bills.CountAsync(ct);

        var items = await bills
            .OrderByDescending(b => b.BillDate)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task<Bill?> GetByIdAsync(int billId, CancellationToken ct = default) =>
        _db.Bills.AsNoTracking()
            .Include(b => b.Car)
            .Include(b => b.Customer)
            .FirstOrDefaultAsync(b => b.BillId == billId, ct);

    public async Task AddAsync(Bill bill, CancellationToken ct = default) =>
        await _db.Bills.AddAsync(bill, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);
}