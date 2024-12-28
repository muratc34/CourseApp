namespace Infrastructure.Configurations;

internal class CourseOrderConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(oc => new {oc.CourseId, oc.OrderId});

        builder.HasOne(oc => oc.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(oc => oc.OrderId);
        
        builder.HasOne(oc => oc.Course)
            .WithMany()
            .HasForeignKey(oc => oc.CourseId);
    }
}
