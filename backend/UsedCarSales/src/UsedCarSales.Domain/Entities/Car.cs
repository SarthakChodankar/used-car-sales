using System;
using System.Collections.Generic;

namespace UsedCarSales.Domain.Entities;

public partial class Car
{
    public int CarId { get; set; }

    public string Vin { get; set; } = null!;

    public string Make { get; set; } = null!;

    public string Model { get; set; } = null!;

    public short ManufactureYear { get; set; }

    public int Mileage { get; set; }

    public string? Color { get; set; }

    public string FuelType { get; set; } = null!;

    public string Transmission { get; set; } = null!;

    public decimal Price { get; set; }

    public string Status { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Bill? Bill { get; set; }

   

    public virtual ICollection<CarImage> CarImages { get; set; } = new List<CarImage>();
}
