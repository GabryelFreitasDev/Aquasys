// Na pasta Data/AppDbContext.cs
using Aquasys.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Aquasys.WebApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Adicione um DbSet para CADA entidade sincronizável
    public DbSet<Vessel> Vessels { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Hold> Holds { get; set; }
    public DbSet<HoldInspectionImage> HoldImages { get; set; }
    public DbSet<HoldInspection> HoldInspections { get; set; }
    public DbSet<VesselImage> VesselImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vessel>()
            .HasMany(v => v.Holds)
            .WithOne(h => h.VesselEntity)
            .HasForeignKey(h => h.IDVessel);

        modelBuilder.Entity<Vessel>()
            .HasMany(v => v.VesselImages)
            .WithOne(i => i.VesselEntity)
            .HasForeignKey(i => i.IDVessel);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clr = entityType.ClrType;

            if (!typeof(SyncableEntity).IsAssignableFrom(clr))
                continue;

            // índice único pro GlobalId
            modelBuilder.Entity(clr)
                .HasIndex(nameof(SyncableEntity.GlobalId))
                .IsUnique();

            // descobrir a PK
            var pk = entityType.FindPrimaryKey();
            var pkProp = pk?.Properties.FirstOrDefault();
            if (pkProp == null)
                continue;

            // se a PK for numérica, não deixar EF gerar valor
            if (pkProp.ClrType == typeof(long) || pkProp.ClrType == typeof(int))
            {
                modelBuilder.Entity(clr)
                    .Property(pkProp.Name)
                    .ValueGeneratedNever();  // <- ESSA É A CHAVE
            }
        }
    }
}