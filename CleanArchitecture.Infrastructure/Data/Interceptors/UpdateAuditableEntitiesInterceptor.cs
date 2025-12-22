using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public sealed class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    private readonly IUserContext _userContext;

    public UpdateAuditableEntitiesInterceptor(IUserContext userContext)
    {
        _userContext = userContext;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = dbContext.ChangeTracker
            .Entries<IAuditableEntity>();

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Entity.CreatedOnUtc = DateTime.UtcNow;
                entityEntry.Entity.CreatedBy = _userContext.UserEmail;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Entity.ModifiedOnUtc = DateTime.UtcNow;
                entityEntry.Entity.ModifiedBy = _userContext.UserEmail;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}