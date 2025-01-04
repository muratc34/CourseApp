namespace Infrastructure.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
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
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .IsRequired();

        builder.Property(c => c.Price)
            .IsRequired();

        builder.Property(c => c.ImageUrl);

        builder.HasOne(c => c.Category)
            .WithMany(c => c.Courses)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Instructor)
            .WithMany(i => i.CoursesCreated)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
