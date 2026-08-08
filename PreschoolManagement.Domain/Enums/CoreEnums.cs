namespace PreschoolManagement.Domain.Enums;

public enum UserRole
{
    SuperAdmin = 1,
    PreschoolAdmin = 2,
    Teacher = 3,
    Parent = 4
}

public enum Gender
{
    Male = 1,
    Female = 2,
    Other = 3
}

public enum AttendanceStatus
{
    Present = 1,
    Absent = 2,
    Late = 3,
    HalfDay = 4
}

public enum PaymentStatus
{
    Pending = 1,
    Paid = 2,
    Failed = 3,
    Refunded = 4
}

public enum PaymentMethod
{
    Cash = 1,
    Card = 2,
    BankTransfer = 3,
    Upi = 4,
    Wallet = 5
}

public enum NoticeTargetAudience
{
    All = 1,
    Parents = 2,
    Teachers = 3,
    Admins = 4
}

public enum NotificationType
{
    Push = 1,
    Email = 2,
    AttendanceAlert = 3,
    FeeReminder = 4,
    NoticeAlert = 5
}

public enum StudentStatus
{
    Active = 1,
    Transferred = 2,
    Inactive = 3
}
