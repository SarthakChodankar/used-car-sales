using UsedCarSales.Application.DTOs.Cars;
using UsedCarSales.Domain.Entities;

namespace UsedCarSales.Application.Interfaces;

public interface ICarRepository
{
    Task<(IReadOnlyList<Car> Items, int TotalCount)> GetPagedAsync(
        CarQueryParams query, CancellationToken ct = default);

    Task<Car?> GetByIdAsync(int carId, CancellationToken ct = default);

    Task<bool> VinExistsAsync(string vin, int? excludeCarId, CancellationToken ct = default);

    Task AddAsync(Car car, CancellationToken ct = default);

    void Remove(Car car);

    void AddImage(CarImage image);

    void RemoveImage(CarImage image);
    Task SaveChangesAsync(CancellationToken ct = default);
}