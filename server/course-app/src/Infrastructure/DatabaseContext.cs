using System;
using System.Data;

namespace Infrastructure;

public class DatabaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<ApplicationRole> ApplicationRole { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Domain.Entities.Order> Orders { get; set; }
    public DbSet<Domain.Entities.Payment> Payments { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        long utcNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        UpdateAuditableEntities(utcNow);
        UpdateSoftDeletableEntities(utcNow);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableEntities(long utcNow)
    {

        IEnumerable<EntityEntry<IAuditableEntity>> entries = ChangeTracker.Entries<IAuditableEntity>();

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
        IEnumerable<EntityEntry<ISoftDeletableEntity>> entries = ChangeTracker.Entries<ISoftDeletableEntity>();

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

        foreach (ReferenceEntry referenceEntry in entityEntry.References.Where(r => r.TargetEntry?.State == EntityState.Deleted))
        {
            referenceEntry.TargetEntry.State = EntityState.Unchanged;

            UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
        }
    }

    private static void RestoreDeletedEntityEntryReferences(EntityEntry entityEntry)
    {
        if (!entityEntry.References.Any())
        {
            return;
        }

        foreach (var referenceEntry in entityEntry.References.Where(r => r.TargetEntry != null && r.TargetEntry.State == EntityState.Modified))
        {
            referenceEntry.TargetEntry.State = EntityState.Modified;

            RestoreDeletedEntityEntryReferences(referenceEntry.TargetEntry);
        }
    }
}
