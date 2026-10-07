using System;
using System.Collections.Generic;

namespace UsedCarSales.Domain.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
