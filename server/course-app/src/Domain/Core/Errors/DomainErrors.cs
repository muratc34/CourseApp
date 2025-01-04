namespace Domain.Core.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static Error NotFound(Guid userId) => Error.NotFound("User.NotFound", $"The user with the Id = '{userId}' was not found.");
        public static Error Permission => Error.Permission("User.InvalidPermissions", "The current user does not have the permissions to perform that operation.");
        public static Error DuplicateEmailOrUserName => Error.Conflict("User.DuplicateEmailOrUserName", "The specified email or username is already in use.");
        public static Error CannotCreate(string message) => Error.Failure("User.CannotCreate", message);
        public static Error CannotUpdate(string message) => Error.Failure("User.CannotUpdate", message);
        public static Error CannotDelete(string message) => Error.Failure("User.CannotDelete", message);
        public static Error AlreadyEnrollment => Error.Conflict("User.AlreadyEnrollment", "The specified user is already enrollment to specified course.");
        public static Error EmailConfirmationOTPInvalid => Error.Conflict("User.EmailConfirmationOTPInvalid", "The code is incorrect or expired");
        public static Error EmailAlreadyConfirmed => Error.Conflict("User.EmailAlreadyConfirmed", "The email is already confirmed.");
        public static Error ProfilePictureUrlAlreadyDeleted => Error.Failure("User.ProfilePictureUrlAlreadyDeleted", "The profile picture url already deleted.");
    }
    public static class Role
    {
        public static Error NotFound(Guid roleId) => Error.NotFound("Role.NotFound", $"The role with the Id = '{roleId}' was not found.");
    }
    public static class Authentication
    {
        public static Error InvalidPermissions => Error.Failure("Authentication.InvalidPermissions", "The current user does not have the permissions to perform that operation.");
        public static Error InvalidEmailOrPassword => Error.Failure("Authentication.InvalidEmailOrPassword", "The specified email or password are incorrect.");
        public static Error CannotChangePassword(string message) => Error.Failure("Authentication.CannotChangePassword", message);
    }
    public static class RefreshToken
    {
        public static Error NotFound => Error.NotFound("RefreshToken.NotFound", "The refresh token is not exist.");
        public static Error TokenExpired => Error.NotFound("RefreshToken.TokenExpired", "The refresh token has expired.");
    }

    public static class Category
    {
        public static Error NotFound => Error.NotFound("Category.NotFound", "The category is not exist.");
    }
    public static class Course
    {
        public static Error NotFound => Error.NotFound("Course.NotFound", "The course is not exist.");
        public static Error CannotUpdate(string desc) => Error.Failure("Course.CannotUpdate", desc);
        public static Error OwnerCannotOrder => Error.Failure("Course.OwnerCannotOrder", "The course owner cannot buy the course.");
        public static Error ImageUrlAlreadyDeleted => Error.Failure("Course.OwnerCannotOrder", "The image url already deleted.");
    }
    public static class Order
    {
        public static Error NotFound => Error.NotFound("Order.NotFound", "The order is not exist.");
        public static Error AlreadyOrdered => Error.Conflict("Order.AlreadyOrdered", "The course is already ordered.");
    }
    public static class Payment
    {
        public static Error AlreadyPaid => Error.Failure("Payment.AlreadyPaid", "The order has already been paid for.");
        public static Error Failed => Error.Failure("Payment.Failed", "The payment failed.");
    }
}
