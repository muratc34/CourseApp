namespace Application.Abstractions.Notifications.Contracts;

public sealed class CoursePurchase
{
    public CoursePurchase(string courseName, string studentName, string instructorName, string instructorEmail)
    {
        CourseName = courseName;
        StudentName = studentName;
        InstructorName = instructorName;
        InstructorEmail = instructorEmail;
    }
    public string CourseName { get; private set; }
    public string StudentName { get; private set; }
    public string InstructorName { get; private set; }
    public string InstructorEmail { get; private set; }
}