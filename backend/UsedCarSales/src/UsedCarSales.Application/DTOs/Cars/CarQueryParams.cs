namespace UsedCarSales.Application.DTOs.Cars;

public class CarQueryParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;

    public string? Search { get; set; }
    public string? Status { get; set; }
    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value is < 1 ? 10 : Math.Min(value, MaxPageSize);
    }
}