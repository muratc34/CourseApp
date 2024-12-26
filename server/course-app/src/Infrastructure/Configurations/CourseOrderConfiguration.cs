namespace Infrastructure.Configurations;

internal class CourseOrderConfiguration : IEntityTypeConfiguration<CourseOrder>
{
    public void Configure(EntityTypeBuilder<CourseOrder> builder)
    {
        builder.HasKey(oc => new {oc.CourseId, oc.OrderId});

        builder.HasOne(oc => oc.Order)
            .WithMany(o => o.CourseOrders)
            .HasForeignKey(oc => oc.OrderId);
        
        builder.HasOne(oc => oc.Course)
            .WithMany(c => c.CourseOrders)
            .HasForeignKey(oc => oc.CourseId);
    }
}
