using UsedCarSales.Application.DTOs.Cars;

namespace UsedCarSales.Application.Interfaces;

public interface ICarImageService
{
    Task<CarImageDto> AddAsync(int carId, Stream content, string fileName,
        string contentType, long length, CancellationToken ct = default);

    Task SetPrimaryAsync(int carId, int imageId, CancellationToken ct = default);

    Task DeleteAsync(int carId, int imageId, CancellationToken ct = default);
}