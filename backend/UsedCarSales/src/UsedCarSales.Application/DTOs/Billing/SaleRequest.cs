using System.ComponentModel.DataAnnotations;

namespace UsedCarSales.Application.DTOs.Billing;

public class SaleRequest
{
    [Range(1, int.MaxValue)]
    public int CarId { get; set; }

    [Range(1, int.MaxValue)]
    public int CustomerId { get; set; }

    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal SalePrice { get; set; }

    [Required, RegularExpression("^(Cash|Card|BankTransfer|Finance)$",
        ErrorMessage = "PaymentMethod must be Cash, Card, BankTransfer or Finance.")]
    public string PaymentMethod { get; set; } = string.Empty;
}