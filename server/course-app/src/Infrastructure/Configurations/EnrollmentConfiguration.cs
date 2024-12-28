namespace Infrastructure.Configurations;

internal class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => new {e.UserId, e.CourseId});

        builder.Property(c => c.CreatedOnUtc)
            .IsRequired();

        builder.Property(c => c.ModifiedOnUtc);

        builder.HasOne(e => e.User)
            .WithMany(u => u.Enrollments)
            .HasForeignKey(e => e.UserId);

        builder.HasOne(e => e.Course)
            .WithMany(c=> c.Enrollments)
            .HasForeignKey(e => e.CourseId);
    }
}
