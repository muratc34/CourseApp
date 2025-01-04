namespace Infrastructure.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Domain.Entities.Order>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.CreatedOnUtc)
            .IsRequired();

        builder.Property(o => o.ModifiedOnUtc);

        builder.Property(o => o.DeletedOnUtc);

        builder.Property(o => o.Deleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(c => !c.Deleted);

        builder.Property(o => o.TcNo)
            .IsRequired();

        builder.Property(o => o.City)
            .IsRequired();

        builder.Property(o => o.Country)
            .IsRequired();

        builder.Property(o => o.Address)
            .IsRequired();

        builder.Property(o => o.ZipCode)
            .IsRequired();

        builder.Property(o => o.UserId)
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}