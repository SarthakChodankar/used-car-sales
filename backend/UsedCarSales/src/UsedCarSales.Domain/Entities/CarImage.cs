using System;
using System.Collections.Generic;

namespace UsedCarSales.Domain.Entities;

public partial class CarImage
{
    public int ImageId { get; set; }

    public int CarId { get; set; }

    public string FilePath { get; set; } = null!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Car Car { get; set; } = null!;
}
