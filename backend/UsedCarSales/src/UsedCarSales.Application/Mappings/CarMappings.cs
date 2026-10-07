using UsedCarSales.Application.DTOs.Cars;
using UsedCarSales.Domain.Entities;

namespace UsedCarSales.Application.Mappings;

public static class CarMappings
{
    public static CarDto ToDto(this Car car)
    {
        var images = car.CarImages
            .OrderByDescending(i => i.IsPrimary)
            .ThenBy(i => i.ImageId)
            .Select(i => new CarImageDto
            {
                ImageId = i.ImageId,
                Url = i.FilePath,
                IsPrimary = i.IsPrimary
            })
            .ToList();

        return new CarDto
        {
            CarId = car.CarId,
            Vin = car.Vin,
            Make = car.Make,
            Model = car.Model,
            ManufactureYear = car.ManufactureYear,
            Mileage = car.Mileage,
            Color = car.Color,
            FuelType = car.FuelType,
            Transmission = car.Transmission,
            Price = car.Price,
            Status = car.Status,
            Description = car.Description,
            PrimaryImageUrl = images.FirstOrDefault()?.Url,
            Images = images
        };
    }

    public static void ApplyTo(this CarSaveRequest request, Car car)
    {
        car.Vin = request.Vin.Trim().ToUpperInvariant();
        car.Make = request.Make.Trim();
        car.Model = request.Model.Trim();
        car.ManufactureYear = request.ManufactureYear;
        car.Mileage = request.Mileage;
        car.Color = request.Color?.Trim();
        car.FuelType = request.FuelType;
        car.Transmission = request.Transmission;
        car.Price = request.Price;
        car.Description = request.Description?.Trim();
    }
}