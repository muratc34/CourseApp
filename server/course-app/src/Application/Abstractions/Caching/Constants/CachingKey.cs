namespace Application.Abstractions.Caching.Constants;

public static class CachingKeys
{
    public static string EmailVerificationKey(Guid userId) => $"emailverification:{userId}";
    public static string CategoriesKey => $"categories";
    public static string CategoryByIdKey(Guid categoryId) => $"category:{categoryId}";
    public static string CoursesKey(int pageIndex, int pageSize) => $"courses:{pageIndex}:{pageSize}";
    public static string CoursesRemoveKey => $"courses:*";
    public static string CourseByIdKey(Guid courseId) => $"course:{courseId}";
    public static string CoursesByCagetoryIdKey(Guid categoryId, int pageIndex, int pageSize) => $"coursesbycategoryid:{categoryId}:{pageIndex}:{pageSize}";
    public static string CoursesByCagetoryIdRemoveKey(Guid categoryId) => $"coursesbycategoryid:{categoryId}:*";
    public static string UserCoursesByEnrollmentUserIdKey(Guid userId) => $"usercoursesbyenrollmentuserid:{userId}";
    public static string UserCoursesByEnrollmentUserIdRemoveAllKey => $"usercoursesbyenrollmentuserid:*";

    public static string UserCoursesByInstructorUserIdKey(Guid userId) => $"usercoursesbyinstructoruserid:{userId}";
    public static string UserCoursesByInstructorUserIdRemoveAllKey => $"usercoursesbyinstructoruserid:*";
    public static string OrdersByUserIdKey(Guid userId, int pageIndex, int pageSize) => $"ordersbyuserid:{userId}:{pageIndex}:{pageSize}";
    public static string OrdersByUserIdRemoveKey(Guid userId) => $"ordersbyuserid:{userId}:*";
    public static string OrdersByIdKey(Guid orderId) => $"ordersbyid:{orderId}";
}
