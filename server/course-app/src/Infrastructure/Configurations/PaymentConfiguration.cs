namespace Infrastructure.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Domain.Entities.Payment>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.CreatedOnUtc)
            .IsRequired();

        builder.Property(p => p.ModifiedOnUtc);

        builder.Property(p => p.DeletedOnUtc);

        builder.Property(p => p.Deleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(p => !p.Deleted);

        builder.Property(p => p.Amount)
            .IsRequired();

        builder.HasOne(p => p.Order)
            .WithOne(o => o.Payment)
            .HasForeignKey<Domain.Entities.Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
