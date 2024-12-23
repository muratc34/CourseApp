namespace Domain.Entities;

public static class OrderStatuses
{
    public const string Pending = "Pending";
    public const string Processing = "Processing";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";
    public const string Failed = "Failed";
    public const string Refunded = "Refunded";

    public static readonly IReadOnlyCollection<string> AllStatuses = new[]
    {
        Pending,
        Processing,
        Completed,
        Cancelled,
        Failed,
        Refunded
    };
}
