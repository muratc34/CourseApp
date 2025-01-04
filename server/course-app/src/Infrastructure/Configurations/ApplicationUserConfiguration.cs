namespace Infrastructure.Configurations;

public sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.CreatedOnUtc)
            .IsRequired();

        builder.Property(u => u.ModifiedOnUtc);

        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Ignore(u => u.FullName);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.ProfilePictureUrl);

        builder.HasMany(u => u.Orders)
           .WithOne(o => o.User)
           .HasForeignKey(o => o.UserId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}
