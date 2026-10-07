namespace UsedCarSales.Application.DTOs.Billing;

public class BillDto
{
    public int BillId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime BillDate { get; set; }
    public decimal SalePrice { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public BillCarDto Car { get; set; } = new();
    public BillCustomerDto Customer { get; set; } = new();
}

public class BillCarDto
{
    public int CarId { get; set; }
    public string Vin { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public short ManufactureYear { get; set; }
}

public class BillCustomerDto
{
    public int CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
}