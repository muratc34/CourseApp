namespace Infrastructure.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CreatedOnUtc)
            .IsRequired();

        builder.Property(c => c.ModifiedOnUtc);

        builder.Property(c => c.DeletedOnUtc);

        builder.Property(c => c.Deleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(c => !c.Deleted);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
