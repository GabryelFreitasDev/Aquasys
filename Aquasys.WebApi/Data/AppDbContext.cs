// Na pasta Data/AppDbContext.cs
using Aquasys.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aquasys.WebApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Adicione um DbSet para CADA entidade sincronizável
    public DbSet<Vessel> Vessels { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Hold> Holds { get; set; }
    public DbSet<Inspection> Inspections { get; set; }
    public DbSet<TypeVessel> TypeVessels { get; set; }
    public DbSet<HoldCargo> HoldCargos { get; set; }
    public DbSet<HoldInspectionImage> HoldImages { get; set; }
    public DbSet<HoldCondition> HoldConditions { get; set; }
    public DbSet<HoldInspection> HoldInspections { get; set; }
    public DbSet<VesselImage> VesselImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configura um índice no GlobalId para todas as entidades para otimizar a busca.
        // Isso é crucial para a performance do endpoint de Push.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(SyncableEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType).HasIndex(nameof(SyncableEntity.GlobalId)).IsUnique();
            }
        }
    }
}