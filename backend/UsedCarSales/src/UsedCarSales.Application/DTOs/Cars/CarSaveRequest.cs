using System.ComponentModel.DataAnnotations;

namespace UsedCarSales.Application.DTOs.Cars;

public class CarSaveRequest
{
    [Required, StringLength(17, MinimumLength = 11)]
    public string Vin { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Make { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Model { get; set; } = string.Empty;

    [Range(1980, 2100)]
    public short ManufactureYear { get; set; }

    [Range(0, int.MaxValue)]
    public int Mileage { get; set; }

    [StringLength(30)]
    public string? Color { get; set; }

    [Required, RegularExpression("^(Petrol|Diesel|CNG|Electric|Hybrid)$",
        ErrorMessage = "FuelType must be Petrol, Diesel, CNG, Electric or Hybrid.")]
    public string FuelType { get; set; } = string.Empty;

    [Required, RegularExpression("^(Manual|Automatic)$",
        ErrorMessage = "Transmission must be Manual or Automatic.")]
    public string Transmission { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal Price { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }
}