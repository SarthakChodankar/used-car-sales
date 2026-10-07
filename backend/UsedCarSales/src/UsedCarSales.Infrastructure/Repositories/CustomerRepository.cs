using Microsoft.EntityFrameworkCore;
using UsedCarSales.Application.DTOs.Customers;
using UsedCarSales.Application.Interfaces;
using UsedCarSales.Domain.Entities;
using UsedCarSales.Infrastructure.Persistence;

namespace UsedCarSales.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db) => _db = db;

    public async Task<(IReadOnlyList<Customer> Items, int TotalCount)> GetPagedAsync(
        CustomerQueryParams query, CancellationToken ct = default)
    {
        var customers = _db.Customers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim();
            customers = customers.Where(c => c.FullName.Contains(term)
                                          || c.Email.Contains(term)
                                          || c.Phone.Contains(term));
        }

        var total = await customers.CountAsync(ct);

        var items = await customers
            .OrderBy(c => c.FullName)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task<Customer?> GetByIdAsync(int customerId, CancellationToken ct = default) =>
        _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId, ct);

    public Task<bool> EmailExistsAsync(string email, int? excludeCustomerId, CancellationToken ct = default) =>
        _db.Customers.AnyAsync(c => c.Email == email
            && (excludeCustomerId == null || c.CustomerId != excludeCustomerId), ct);

    public Task<bool> HasBillsAsync(int customerId, CancellationToken ct = default) =>
        _db.Bills.AnyAsync(b => b.CustomerId == customerId, ct);

    public async Task AddAsync(Customer customer, CancellationToken ct = default) =>
        await _db.Customers.AddAsync(customer, ct);

    public void Remove(Customer customer) => _db.Customers.Remove(customer);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);
}