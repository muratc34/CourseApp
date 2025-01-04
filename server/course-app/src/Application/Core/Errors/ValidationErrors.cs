namespace Application.Core.Errors;

internal static class ValidationErrors
{
    internal static class LoginDto
    {
        internal static Error EmailRequired => Error.Failure("LoginDto.EmailIsRequired", "The email is required.");
        internal static Error EmailCanNotBeNull => Error.Failure("LoginDto.EmailCanNotBeNull", "The email can not be null.");
        internal static Error PasswordIsRequired => Error.Failure("LoginDto.PasswordIsRequired", "The password is required.");
        internal static Error PasswordCanNotBeNull => Error.Failure("LoginDto.PasswordCanNotBeNull", "The password can not be null.");
    }
    internal static class ChangePasswordDto
    {
        internal static Error OldPasswordIsRequired => Error.Failure("ChangePasswordDto.OldPasswordIsRequired", "The old password is required.");
        internal static Error OldPasswordCanNotBeNull => Error.Failure("ChangePasswordDto.OldPasswordCanNotBeNull", "The old password can not be null.");
        internal static Error NewPasswordIsRequired => Error.Failure("ChangePasswordDto.NewPasswordIsRequired", "The new password is required.");
        internal static Error NewPasswordCanNotBeNull => Error.Failure("ChangePasswordDto.NewPasswordCanNotBeNull", "The new password can not be null.");
    }
    internal static class CategorySaveDto
    {
        internal static Error NameIsRequired => Error.Failure("CategorySaveDto.NameIsRequired", "The old password is required.");
        internal static Error NameCanNotBeNull => Error.Failure("CategorySaveDto.NameCanNotBeNull", "The old password can not be null.");
    }
    internal static class CourseCreateDto
    {
        internal static Error CategoryIdIsRequired => Error.Failure("CourseCreateDto.CategoryIdIsRequired", "The category id is required.");
        internal static Error CategoryIdCanNotBeNull => Error.Failure("CourseCreateDto.CategoryIdCanNotBeNull", "The category id can not be null.");
        internal static Error DescriptionIsRequired => Error.Failure("CourseCreateDto.Description", "The description is required.");
        internal static Error DescriptionCanNotBeNull => Error.Failure("CourseCreateDto.DescriptionCanNotBeNull", "The description can not be null.");
        internal static Error InstructorIdIsRequired => Error.Failure("CourseCreateDto.InstructorIdIsRequired", "The instructor id is required.");
        internal static Error InstructorIdCanNotBeNull => Error.Failure("CourseCreateDto.InstructorIdCanNotBeNull", "The instructor id can not be null.");
        internal static Error NameIsRequired => Error.Failure("CourseCreateDto.NameIsRequired", "The name is required.");
        internal static Error NameCanNotBeNull => Error.Failure("CourseCreateDto.NameCanNotBeNull", "The name can not be null.");
        internal static Error PriceIsRequired => Error.Failure("CourseCreateDto.PriceIsRequired", "The price is required.");
        internal static Error PriceCanNotBeNull => Error.Failure("CourseCreateDto.PriceCanNotBeNull", "The price can not be null.");
        internal static Error PriceMustBeGreaterThanZero => Error.Failure("CourseCreateDto.PriceMustBeGreaterThanZero", "The price must be greater than 0.");
    }
    internal static class OrderCreateDto
    {
        internal static Error UserIdRequired => Error.Failure("OrderCreateDto.UserIdRequired", "The user id is required.");
        internal static Error UserIdCanNotBeNull => Error.Failure("OrderCreateDto.UserIdCanNotBeNull", "The user id can not be null.");
        internal static Error CourseIdsRequired => Error.Failure("OrderCreateDto.CourseIdsRequired", "At least one course must be selected");
        internal static Error CourseIdsCanNotBeNull => Error.Failure("OrderCreateDto.CourseIdsCanNotBeNull", "The category id array can not be null.");
        internal static Error CityIsRequired => Error.Failure("OrderCreateDto.CityIsRequired", "The city is required.");
        internal static Error CityCanNotBeNull => Error.Failure("OrderCreateDto.CityCanNotBeNull", "The category can not be null.");
        internal static Error CountryIsRequired => Error.Failure("OrderCreateDto.CountryIsRequired", "The country is required.");
        internal static Error CountryCanNotBeNull => Error.Failure("OrderCreateDto.CountryCanNotBeNull", "The country can not be null.");
        internal static Error AddressIsRequired => Error.Failure("OrderCreateDto.AddressIsRequired", "The address is required.");
        internal static Error AddressCanNotBeNull => Error.Failure("OrderCreateDto.AddressCanNotBeNull", "The address can not be null.");
        internal static Error ZipCodeIsRequired => Error.Failure("OrderCreateDto.ZipCodeIsRequired", "The zip code is required.");
        internal static Error ZipCodeCanNotBeNull => Error.Failure("OrderCreateDto.ZipCodeCanNotBeNull", "The zip code can not be null.");
        internal static Error ZipCodeMustBeFiveDigitNumber => Error.Failure("OrderCreateDto.ZipCodeMustBeFiveDigitNumber", "Zip code must be a 5-digit number.");
        internal static Error TcNoIsRequired => Error.Failure("OrderCreateDto.TcNoIsRequired", "The TC number is required.");
        internal static Error TcNoCanNotBeNull => Error.Failure("OrderCreateDto.TcNoCanNotBeNull", "The TC number can not be null.");
        internal static Error TcNoLength => Error.Failure("OrderCreateDto.TcNoLength", "TC number must be 11 characters long.");
        internal static Error TcNoDigitsMustBeNumber => Error.Failure("OrderCreateDto.TcNoDigitsMustBeNumber", "TC number must consist only of digits.");
    }
    internal static class PaymentCreateDto
    {
        internal static Error OrderIdIsRequired => Error.Failure("PaymentCreateDto.OrderIdIsRequired", "The order id is required.");
        internal static Error OrderIdCanNotBeNull => Error.Failure("PaymentCreateDto.OrderIdCanNotBeNull", "The order id can not be null.");
    }
    internal static class UserCreateDto
    {
        internal static Error EmailIsRequired => Error.Failure("UserCreateDto.EmailIsRequired", "The email is required.");
        internal static Error EmailCanNotBeNull => Error.Failure("UserCreateDto.EmailCanNotBeNull", "The email can not be null.");
        internal static Error FirstNameIsRequired => Error.Failure("UserCreateDto.FirstNameIsRequired", "The first name is required.");
        internal static Error FirstNameCanNotBeNull => Error.Failure("UserCreateDto.FirstNameCanNotBeNull", "The first name can not be null.");
        internal static Error LastNameIsRequired => Error.Failure("UserCreateDto.LastNameIsRequired", "The last name is required.");
        internal static Error LastNameCanNotBeNull => Error.Failure("UserCreateDto.LastNameCanNotBeNull", "The last name can not be null.");
        internal static Error PasswordIsRequired => Error.Failure("UserCreateDto.PasswordIsRequired", "The password is required.");
        internal static Error PasswordCanNotBeNull => Error.Failure("UserCreateDto.PasswordCanNotBeNull", "The password can not be null.");
        internal static Error UserNameIsRequired => Error.Failure("UserCreateDto.UserNameIsRequired", "The username is required.");
        internal static Error UserNameCanNotBeNull => Error.Failure("UserCreateDto.UserNameCanNotBeNull", "The username can not be null.");
    }
    internal static class UserUpdateDto
    {
        internal static Error EmailIsRequired => Error.Failure("UserUpdateDto.EmailIsRequired", "The email is required.");
        internal static Error FirstNameIsRequired => Error.Failure("UserUpdateDto.FirstNameIsRequired", "The first name is required.");
        internal static Error LastNameIsRequired => Error.Failure("UserUpdateDto.LastNameIsRequired", "The last name is required.");
        internal static Error UserNameIsRequired => Error.Failure("UserUpdateDto.UserNameIsRequired", "The username is required.");

    }
    internal static class RoleSaveDto
    {
        internal static Error NameIsRequired => Error.Failure("RoleSaveDto.NameIsRequired", "The name is required.");
    }
    internal static class Course
    {
        internal static Error SearchNameIsRequired => Error.Failure("Course.SearchNameIsRequired", "The search name is required.");
    }
}


