using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UsedCarSales.Domain.Entities;

namespace UsedCarSales.Infrastructure.Persistence;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarImage> CarImages { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_Bills_CustomerId");

            entity.HasIndex(e => e.CarId, "UQ_Bills_CarId").IsUnique();

            entity.HasIndex(e => e.InvoiceNumber, "UQ_Bills_InvoiceNumber").IsUnique();

            entity.Property(e => e.BillDate).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.InvoiceNumber).HasMaxLength(30);
            entity.Property(e => e.PaymentMethod).HasMaxLength(20);
            entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Car).WithOne(p => p.Bill)
                .HasForeignKey<Bill>(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bills_Cars");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bills)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bills_Customers");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasIndex(e => new { e.Status, e.Make }, "IX_Cars_Status_Make");

            entity.HasIndex(e => e.Vin, "UQ_Cars_Vin").IsUnique();

            entity.Property(e => e.Color).HasMaxLength(30);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.FuelType).HasMaxLength(20);
            entity.Property(e => e.Make).HasMaxLength(50);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Available");
            entity.Property(e => e.Transmission).HasMaxLength(20);
            entity.Property(e => e.Vin).HasMaxLength(17);
        });

        modelBuilder.Entity<CarImage>(entity =>
        {
            entity.HasKey(e => e.ImageId);

            entity.HasIndex(e => e.CarId, "IX_CarImages_CarId");

            entity.HasIndex(e => e.CarId, "UX_CarImages_OnePrimary")
                .IsUnique()
                .HasFilter("([IsPrimary]=(1))");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.FilePath).HasMaxLength(500);

            entity.HasOne(d => d.Car).WithMany(p => p.CarImages)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK_CarImages_Cars");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.Email, "UQ_Customers_Email").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
