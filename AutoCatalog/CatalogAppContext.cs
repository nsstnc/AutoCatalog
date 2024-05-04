using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AutoCatalog;

public partial class CatalogAppContext : DbContext
{
    public CatalogAppContext()
    {
    }

    public CatalogAppContext(DbContextOptions<CatalogAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Configuration> Configurations { get; set; }

    public virtual DbSet<Engine> Engines { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<SuspensionAndBrake> SuspensionAndBrakes { get; set; }

    public virtual DbSet<Transmission> Transmissions { get; set; }








    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
        optionsBuilder.UseNpgsql(ConfigManager.LoadConfig().DatabaseConnectionString); 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Car_pkey");

            entity.ToTable("Car", "catalog_app");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.ConfigurationId).HasColumnName("configurationId");
            entity.Property(e => e.Generation).HasColumnName("generation");
            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturerId");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Year).HasColumnName("year");

            entity.HasOne(d => d.Configuration).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ConfigurationId)
                .HasConstraintName("Car_configurationId_fkey");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("Car_manufacturerId_fkey");
        });

        modelBuilder.Entity<Configuration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Configuration_pkey");

            entity.ToTable("Configuration", "catalog_app");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Clearance).HasColumnName("clearance");
            entity.Property(e => e.CurbWeight).HasColumnName("curbWeight");
            entity.Property(e => e.EngineId).HasColumnName("engineId");
            entity.Property(e => e.FuelTankVolume).HasColumnName("fuelTankVolume");
            entity.Property(e => e.FullWeight).HasColumnName("fullWeight");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.NumberOfSeats).HasColumnName("numberOfSeats");
            entity.Property(e => e.OverClocking).HasColumnName("overClocking");
            entity.Property(e => e.SuspensionAndBrakesId).HasColumnName("suspensionAndBrakesId");
            entity.Property(e => e.TransmissionId).HasColumnName("transmissionId");
            entity.Property(e => e.TypeOfDrive).HasColumnName("typeOfDrive");

            entity.HasOne(d => d.Engine).WithMany(p => p.Configurations)
                .HasForeignKey(d => d.EngineId)
                .HasConstraintName("Configuration_engineId_fkey");

            entity.HasOne(d => d.SuspensionAndBrakes).WithMany(p => p.Configurations)
                .HasForeignKey(d => d.SuspensionAndBrakesId)
                .HasConstraintName("Configuration_suspensionAndBrakesId_fkey");

            entity.HasOne(d => d.Transmission).WithMany(p => p.Configurations)
                .HasForeignKey(d => d.TransmissionId)
                .HasConstraintName("Configuration_transmissionId_fkey");
        });

        modelBuilder.Entity<Engine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Engine_pkey");

            entity.ToTable("Engine", "catalog_app");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CylinderArrangement).HasColumnName("cylinderArrangement");
            entity.Property(e => e.EnginePowerSupplySystem).HasColumnName("enginePowerSupplySystem");
            entity.Property(e => e.FuelGrade).HasColumnName("fuelGrade");
            entity.Property(e => e.MaxTorque).HasColumnName("maxTorque");
            entity.Property(e => e.NumberOfCylinders).HasColumnName("numberOfCylinders");
            entity.Property(e => e.Power).HasColumnName("power");
            entity.Property(e => e.TypeOfBoost).HasColumnName("typeOfBoost");
            entity.Property(e => e.TypeOfEngine).HasColumnName("typeOfEngine");
            entity.Property(e => e.Volume).HasColumnName("volume");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Manufacturer_pkey");

            entity.ToTable("Manufacturer", "catalog_app");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.YearOfFoundation).HasColumnName("yearOfFoundation");
        });

        modelBuilder.Entity<SuspensionAndBrake>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SuspensionAndBrakes_pkey");

            entity.ToTable("SuspensionAndBrakes", "catalog_app");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BackBrakes).HasColumnName("backBrakes");
            entity.Property(e => e.FrontBrakes).HasColumnName("frontBrakes");
            entity.Property(e => e.TypeOfBackSuspension).HasColumnName("typeOfBackSuspension");
            entity.Property(e => e.TypeOfFrontSuspension).HasColumnName("typeOfFrontSuspension");
        });

        modelBuilder.Entity<Transmission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Transmission_pkey");

            entity.ToTable("Transmission", "catalog_app");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.NumberOfGears).HasColumnName("numberOfGears");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
