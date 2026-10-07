using UsedCarSales.Application.DTOs.Cars;
using UsedCarSales.Application.Interfaces;
using UsedCarSales.Domain.Entities;
using UsedCarSales.Domain.Exceptions;

namespace UsedCarSales.Application.Services;

public class CarImageService : ICarImageService
{
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
    private const int MaxImagesPerCar = 10;
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

    private readonly ICarRepository _cars;
    private readonly IFileStorage _storage;

    public CarImageService(ICarRepository cars, IFileStorage storage)
    {
        _cars = cars;
        _storage = storage;
    }

    public async Task<CarImageDto> AddAsync(int carId, Stream content, string fileName,
        string contentType, long length, CancellationToken ct = default)
    {
        var car = await _cars.GetByIdAsync(carId, ct)
                  ?? throw new NotFoundException("Car", carId);

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension) || !contentType.StartsWith("image/"))
            throw new BusinessRuleException("Only JPG, PNG or WEBP images are allowed.");

        if (length <= 0 || length > MaxFileSizeBytes)
            throw new BusinessRuleException("Image size must be between 1 byte and 5 MB.");

        if (car.CarImages.Count >= MaxImagesPerCar)
            throw new BusinessRuleException($"A car can have at most {MaxImagesPerCar} images.");

        var path = await _storage.SaveAsync(content, extension, ct);

        var image = new CarImage
        {
            CarId = carId,
            FilePath = path,
            IsPrimary = !car.CarImages.Any(),   // first image becomes primary
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            _cars.AddImage(image);
            await _cars.SaveChangesAsync(ct);
        }
        catch
        {
            _storage.Delete(path);   // do not leave an orphan file if the DB save fails
            throw;
        }

        return new CarImageDto { ImageId = image.ImageId, Url = image.FilePath, IsPrimary = image.IsPrimary };
    }

    public async Task SetPrimaryAsync(int carId, int imageId, CancellationToken ct = default)
    {
        var car = await _cars.GetByIdAsync(carId, ct)
                  ?? throw new NotFoundException("Car", carId);

        var image = car.CarImages.FirstOrDefault(i => i.ImageId == imageId)
                    ?? throw new NotFoundException("Image", imageId);

        if (image.IsPrimary) return;

        // Two saves on purpose: the database allows only one primary image per car
        // (filtered unique index), so the old one must be cleared first.
        foreach (var other in car.CarImages.Where(i => i.IsPrimary))
            other.IsPrimary = false;
        await _cars.SaveChangesAsync(ct);

        image.IsPrimary = true;
        await _cars.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int carId, int imageId, CancellationToken ct = default)
    {
        var car = await _cars.GetByIdAsync(carId, ct)
                  ?? throw new NotFoundException("Car", carId);

        var image = car.CarImages.FirstOrDefault(i => i.ImageId == imageId)
                    ?? throw new NotFoundException("Image", imageId);

        var wasPrimary = image.IsPrimary;
        var path = image.FilePath;

        _cars.RemoveImage(image);
        await _cars.SaveChangesAsync(ct);
        _storage.Delete(path);

        // If the primary image was deleted, promote the oldest remaining one.
        if (wasPrimary)
        {
            var next = car.CarImages.OrderBy(i => i.ImageId).FirstOrDefault();
            if (next is not null)
            {
                next.IsPrimary = true;
                await _cars.SaveChangesAsync(ct);
            }
        }
    }
}