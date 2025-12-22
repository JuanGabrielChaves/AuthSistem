using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Common;
using CleanArchitecture.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.DbContexts;

public class ApplicationDbContext : IdentityDbContext<UserIdentity>
{
    private readonly IUserContext? _userContext;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserContext? userContext) : base(options)
    {
        _userContext = userContext;
    }
    public DbSet<Product> Products => Set<Product>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Esto asegura que EF vea las propiedades de la clase base
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.CreatedOnUtc).IsRequired();
            entity.Property(p => p.CreatedBy).HasMaxLength(200);
            entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyAuditInfo();
        return base.SaveChanges();
    }

    private void ApplyAuditInfo()
    {
        // Capturamos los datos una sola vez para todas las entidades
        var timestamp = DateTime.UtcNow;
        var userEmail = _userContext?.UserEmail ?? "System";

        // 1. Buscamos solo las entidades que heredan de BaseEntity y están siendo creadas o editadas
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        Console.WriteLine($"------> Auditando {entries.Count()} entidades en ApplicationDbContext <------");

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedOnUtc = timestamp;
                entry.Entity.CreatedBy = userEmail;
            }
            else // State == EntityState.Modified
            {
                entry.Entity.ModifiedOnUtc = timestamp;
                entry.Entity.ModifiedBy = userEmail;

                // IMPORTANTE: Evita que EF intente actualizar la fecha de creación original
                entry.Property(x => x.CreatedOnUtc).IsModified = false;
                entry.Property(x => x.CreatedBy).IsModified = false;
            }
        }
    }
}