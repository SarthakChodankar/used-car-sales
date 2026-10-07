using System.ComponentModel.DataAnnotations;

namespace UsedCarSales.Application.DTOs.Customers;

public class CustomerSaveRequest
{
    [Required, StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, RegularExpression(@"^[0-9+\-\s]{7,20}$",
        ErrorMessage = "Phone must be 7-20 characters: digits, +, - or spaces.")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Address { get; set; }
}