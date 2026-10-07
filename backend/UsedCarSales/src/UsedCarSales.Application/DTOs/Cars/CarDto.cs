namespace UsedCarSales.Application.DTOs.Cars;

public class CarDto
{
    public int CarId { get; set; }
    public string Vin { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public short ManufactureYear { get; set; }
    public int Mileage { get; set; }
    public string? Color { get; set; }
    public string FuelType { get; set; } = string.Empty;
    public string Transmission { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public List<CarImageDto> Images { get; set; } = new();
}