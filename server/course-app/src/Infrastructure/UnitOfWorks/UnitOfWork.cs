namespace Infrastructure.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _context;

    public UnitOfWork(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        long utcNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        UpdateAuditableEntities(utcNow);
        UpdateSoftDeletableEntities(utcNow);

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result;
    }

    private void UpdateAuditableEntities(long utcNow)
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries =
            _context
                .ChangeTracker
                .Entries<IAuditableEntity>();

        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(nameof(IAuditableEntity.CreatedOnUtc))
                    .CurrentValue = utcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(nameof(IAuditableEntity.ModifiedOnUtc))
                    .CurrentValue = utcNow;
            }
        }
    }

    private void UpdateSoftDeletableEntities(long utcNow)
    {
        IEnumerable<EntityEntry<ISoftDeletableEntity>> entries =
            _context
                .ChangeTracker
                .Entries<ISoftDeletableEntity>();

        foreach (EntityEntry<ISoftDeletableEntity> entityEntry in entries)
        {
            if (entityEntry.State != EntityState.Deleted)
            {
                continue;
            }

            entityEntry.Property(nameof(ISoftDeletableEntity.DeletedOnUtc))
                    .CurrentValue = utcNow;

            entityEntry.Property(nameof(ISoftDeletableEntity.Deleted))
                .CurrentValue = true;

            entityEntry.State = EntityState.Modified;

            UpdateDeletedEntityEntryReferencesToUnchanged(entityEntry);
        }
    }

    private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
    {
        if (!entityEntry.References.Any())
        {
            return;
        }

        foreach (ReferenceEntry referenceEntry in entityEntry.References.Where(r => r.TargetEntry.State == EntityState.Deleted))
        {
            referenceEntry.TargetEntry.State = EntityState.Unchanged;

            UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
        }
    }
}
