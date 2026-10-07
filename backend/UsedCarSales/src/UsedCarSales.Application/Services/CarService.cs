using UsedCarSales.Application.Common;
using UsedCarSales.Application.DTOs.Cars;
using UsedCarSales.Application.Interfaces;
using UsedCarSales.Application.Mappings;
using UsedCarSales.Domain.Constants;
using UsedCarSales.Domain.Entities;
using UsedCarSales.Domain.Exceptions;

namespace UsedCarSales.Application.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _repo;

    public CarService(ICarRepository repo) => _repo = repo;

    public async Task<PagedResult<CarDto>> GetAllAsync(CarQueryParams query, CancellationToken ct = default)
    {
        var (items, total) = await _repo.GetPagedAsync(query, ct);

        return new PagedResult<CarDto>
        {
            Items = items.Select(c => c.ToDto()).ToList(),
            TotalCount = total,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }

    public async Task<CarDto> GetByIdAsync(int carId, CancellationToken ct = default)
    {
        var car = await _repo.GetByIdAsync(carId, ct)
                  ?? throw new NotFoundException("Car", carId);
        return car.ToDto();
    }

    public async Task<CarDto> CreateAsync(CarSaveRequest request, CancellationToken ct = default)
    {
        var vin = request.Vin.Trim().ToUpperInvariant();
        if (await _repo.VinExistsAsync(vin, null, ct))
            throw new BusinessRuleException($"A car with VIN '{vin}' already exists.");

        var car = new Car
        {
            Status = CarStatus.Available,
            CreatedAt = DateTime.UtcNow
        };
        request.ApplyTo(car);

        await _repo.AddAsync(car, ct);
        await _repo.SaveChangesAsync(ct);

        return car.ToDto();
    }

    public async Task UpdateAsync(int carId, CarSaveRequest request, CancellationToken ct = default)
    {
        var car = await _repo.GetByIdAsync(carId, ct)
                  ?? throw new NotFoundException("Car", carId);

        if (car.Status == CarStatus.Sold)
            throw new BusinessRuleException("A sold car cannot be edited.");

        var vin = request.Vin.Trim().ToUpperInvariant();
        if (await _repo.VinExistsAsync(vin, carId, ct))
            throw new BusinessRuleException($"A car with VIN '{vin}' already exists.");

        request.ApplyTo(car);
        car.UpdatedAt = DateTime.UtcNow;

        await _repo.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int carId, CancellationToken ct = default)
    {
        var car = await _repo.GetByIdAsync(carId, ct)
                  ?? throw new NotFoundException("Car", carId);

        if (car.Status == CarStatus.Sold)
            throw new BusinessRuleException("A sold car cannot be deleted because it has billing history.");

        _repo.Remove(car);
        await _repo.SaveChangesAsync(ct);
    }
}