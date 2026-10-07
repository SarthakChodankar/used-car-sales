using System;
using System.Collections.Generic;

namespace UsedCarSales.Domain.Entities;

public partial class Bill
{
    public int BillId { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public int CarId { get; set; }

    public int CustomerId { get; set; }

    public decimal SalePrice { get; set; }

    public decimal TaxPercent { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public DateTime BillDate { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
