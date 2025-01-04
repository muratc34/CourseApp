namespace Infrastructure.Configurations;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.Property(role => role.Name)
           .IsRequired()
           .HasMaxLength(256);

        builder.Property(role => role.CreatedOnUtc)
            .IsRequired();

        builder.Property(role => role.ModifiedOnUtc);
    }
}