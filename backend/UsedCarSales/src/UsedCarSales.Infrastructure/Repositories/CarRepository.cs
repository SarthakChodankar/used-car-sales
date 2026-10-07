using Microsoft.EntityFrameworkCore;
using UsedCarSales.Application.DTOs.Cars;
using UsedCarSales.Application.Interfaces;
using UsedCarSales.Domain.Entities;
using UsedCarSales.Infrastructure.Persistence;

namespace UsedCarSales.Infrastructure.Repositories;

public class CarRepository : ICarRepository
{
    private readonly AppDbContext _db;

    public CarRepository(AppDbContext db) => _db = db;

    public async Task<(IReadOnlyList<Car> Items, int TotalCount)> GetPagedAsync(
        CarQueryParams query, CancellationToken ct = default)
    {
        var cars = _db.Cars.AsNoTracking().Include(c => c.CarImages).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Status))
            cars = cars.Where(c => c.Status == query.Status);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim();
            cars = cars.Where(c => c.Make.Contains(term)
                                || c.Model.Contains(term)
                                || c.Vin.Contains(term));
        }

        var total = await cars.CountAsync(ct);

        var items = await cars
            .OrderByDescending(c => c.CreatedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task<Car?> GetByIdAsync(int carId, CancellationToken ct = default) =>
        _db.Cars.Include(c => c.CarImages).FirstOrDefaultAsync(c => c.CarId == carId, ct);

    public Task<bool> VinExistsAsync(string vin, int? excludeCarId, CancellationToken ct = default) =>
        _db.Cars.AnyAsync(c => c.Vin == vin && (excludeCarId == null || c.CarId != excludeCarId), ct);

    public async Task AddAsync(Car car, CancellationToken ct = default) =>
        await _db.Cars.AddAsync(car, ct);

    public void Remove(Car car) => _db.Cars.Remove(car);

    public void AddImage(CarImage image) => _db.CarImages.Add(image);

    public void RemoveImage(CarImage image) => _db.CarImages.Remove(image);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);
}