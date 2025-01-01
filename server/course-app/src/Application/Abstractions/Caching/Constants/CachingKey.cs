namespace Application.Abstractions.Caching.Constants;

public static class CachingKeys
{
    public static string EmailVerificationKey(Guid userId) => $"emailverification:{userId}";
    public static string CategoriesKey(int pageIndex, int pageSize) => $"categories:{pageIndex}:{pageSize}";
    public static string CategoriesRemoveKey => $"categories:*";
    public static string CategoryByIdKey(Guid categoryId) => $"category:{categoryId}";
    public static string CoursesKey(int pageIndex, int pageSize) => $"courses:{pageIndex}:{pageSize}";
    public static string CoursesRemoveKey => $"courses:*";
    public static string CourseByIdKey(Guid courseId) => $"course:{courseId}";
    public static string CoursesByCagetoryIdKey(Guid categoryId, int pageIndex, int pageSize) => $"coursesbycategoryid:{categoryId}:{pageIndex}:{pageSize}";
    public static string CoursesByCagetoryIdRemoveKey(Guid categoryId) => $"coursesbycategoryid:{categoryId}:*";
    public static string UserCoursesByUserIdKey(Guid userId, int pageIndex, int pageSize) => $"usercoursesbyuserid:{userId}:{pageIndex}:{pageSize}";
    public static string UserCoursesByUserIdRemoveKey(Guid userId) => $"usercoursesbyuserid:{userId}:*";
    public static string UserCoursesByUserIdRemoveAllKey => $"usercoursesbyuserid:*";
    public static string OrdersByUserIdKey(Guid userId, int pageIndex, int pageSize) => $"ordersbyuserid:{userId}:{pageIndex}:{pageSize}";
    public static string OrdersByUserIdRemoveKey(Guid userId) => $"ordersbyuserid:{userId}:*";
    public static string OrdersByIdKey(Guid orderId) => $"ordersbyid:{orderId}";
}
