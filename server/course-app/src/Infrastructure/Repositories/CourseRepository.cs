namespace Infrastructure.Repositories;

internal class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(DatabaseContext context) 
        : base(context)
    {
    }
}
