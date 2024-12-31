namespace Application.Abstractions.Caching.Constants;

public static class CachingKeys
{
    public static string EmailVerificationKey(Guid userId) => $"emailverification:{userId}";
    public static string CategoriesKey => "categories";
    public static string CategoryByIdKey(Guid categoryId) => $"category:{categoryId}";
    public static string CoursesKey => "courses";
    public static string CourseByIdKey(Guid courseId) => $"course:{courseId}";
    public static string CoursesByCagetoryIdKey(Guid categoryId) => $"coursesbycategoryid:{categoryId}";
    public static string UserCoursesByUserIdKey(Guid userId) => $"usercoursesbyuserid:{userId}";
    public static string OrdersByUserIdKey(Guid userId) => $"ordersbyuserid:{userId}";
    public static string OrdersByIdKey(Guid orderId) => $"ordersbyid:{orderId}";
}
