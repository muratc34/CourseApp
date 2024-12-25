namespace Infrastructure.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.CreatedOnUtc)
            .IsRequired();

        builder.Property(r => r.ModifiedOnUtc);

        builder.Property(r => r.DeletedOnUtc);

        builder.Property(r => r.Deleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(r => !r.Deleted);

        builder.Property(r => r.Code)
            .IsRequired();

        builder.Property(r => r.Expiration)
            .IsRequired();
    }
}
