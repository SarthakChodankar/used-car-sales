namespace UsedCarSales.Application.DTOs.Cars;

public class CarImageDto
{
    public int ImageId { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}