using UsedCarSales.Application.Common;
using UsedCarSales.Application.DTOs.Cars;

namespace UsedCarSales.Application.Interfaces;

public interface ICarService
{
    Task<PagedResult<CarDto>> GetAllAsync(CarQueryParams query, CancellationToken ct = default);

    Task<CarDto> GetByIdAsync(int carId, CancellationToken ct = default);

    Task<CarDto> CreateAsync(CarSaveRequest request, CancellationToken ct = default);

    Task UpdateAsync(int carId, CarSaveRequest request, CancellationToken ct = default);

    Task DeleteAsync(int carId, CancellationToken ct = default);
}