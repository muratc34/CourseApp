namespace Domain.Core.Guards;

public static class Ensure
{
    public static void NotEmpty(string value, string message, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotEmpty(Guid value, string message, string argumentName)
    {
        if (value.Equals(Guid.Empty))
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotNegative(decimal value, string message, string argumentName)
    {
        if (value < 0)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotNull<T>(T value, string message, string argumentName)
        where T : class
    {
        if (value is null)
        {
            throw new ArgumentNullException(message, argumentName);
        }
    }

    
}
