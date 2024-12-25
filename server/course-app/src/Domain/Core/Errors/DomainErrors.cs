using Domain.Core.Results;

namespace Domain.Core.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static Error NotFound(Guid userId) => Error.NotFound("User.NotFound", $"The user with the Id = '{userId}' was not found.");
        public static Error Permission => Error.Permission("User.InvalidPermissions", "The current user does not have the permissions to perform that operation.");
        public static Error DuplicateEmail => Error.Conflict("User.DuplicateEmail", "The specified email is already in use.");
        public static Error CannotChangePassword => Error.Failure("User.CannotChangePassword", "The password cannot be changed to the specified password.");
        public static Error CannotCreate(string message) => Error.Failure("User.CannotCreate", message);
    }
    public static class Authentication
    {
        public static Error InvalidEmailOrPassword => Error.Failure("Authentication.InvalidEmailOrPassword", "The specified email or password are incorrect.");
    }
}
