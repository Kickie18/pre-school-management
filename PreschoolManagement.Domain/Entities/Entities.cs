using PreschoolManagement.Domain.Common;
using PreschoolManagement.Domain.Enums;

namespace PreschoolManagement.Domain.Entities;

public class Role : BaseEntity
{
    public string RoleName { get; set; } = string.Empty;
    public ICollection<User> Users { get; set; } = new List<User>();
}

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public string? ProfilePicture { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginDate { get; set; }

    public Role? Role { get; set; }
    public Parent? Parent { get; set; }
    public ICollection<UserSchool> UserSchools { get; set; } = new List<UserSchool>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}

public class School : BaseEntity
{
    public string SchoolName { get; set; } = string.Empty;
    public Guid? AddressId { get; set; }
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Logo { get; set; }

    public Address? Address { get; set; }
    public ICollection<SchoolBranch> Branches { get; set; } = new List<SchoolBranch>();
    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    public ICollection<UserSchool> UserSchools { get; set; } = new List<UserSchool>();
}

public class UserSchool : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid SchoolId { get; set; }

    public User? User { get; set; }
    public School? School { get; set; }
}
public class SchoolBranch : BaseEntity
{
    public Guid SchoolId { get; set; }
    public Guid? AddressId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string BranchCode { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }

    public School? School { get; set; }
    public Address? Address { get; set; }
    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<ClassRoom> ClassRooms { get; set; } = new List<ClassRoom>();
}

public class Address : BaseEntity
{
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public ICollection<School> Schools { get; set; } = new List<School>();
    public ICollection<SchoolBranch> Branches { get; set; } = new List<SchoolBranch>();
    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    public ICollection<Student> Students { get; set; } = new List<Student>();
}

public class Teacher : BaseEntity
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime DOB { get; set; }
    public string Qualification { get; set; } = string.Empty;
    public int Experience { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime JoiningDate { get; set; }
    public string? ProfileImage { get; set; }
    public Guid SchoolId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? AddressId { get; set; }

    public School? School { get; set; }
    public SchoolBranch? Branch { get; set; }
    public Address? Address { get; set; }
    public ICollection<ClassRoom> Classes { get; set; } = new List<ClassRoom>();
    public ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}

public class Parent : BaseEntity
{
    public string FatherName { get; set; } = string.Empty;
    public string MotherName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
    public Guid UserId { get; set; }

    public User? User { get; set; }
    public ICollection<Student> Students { get; set; } = new List<Student>();
}

public class Student : BaseEntity
{
    public string AdmissionNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime DOB { get; set; }
    public string BloodGroup { get; set; } = string.Empty;
    public Guid? AddressId { get; set; }
    public DateTime JoiningDate { get; set; }
    public Guid ClassId { get; set; }
    public Guid ParentId { get; set; }
    public Guid? BranchId { get; set; }
    public string? ProfilePicture { get; set; }
    public StudentStatus Status { get; set; } = StudentStatus.Active;

    public ClassRoom? ClassRoom { get; set; }
    public Parent? Parent { get; set; }
    public SchoolBranch? Branch { get; set; }
    public Address? Address { get; set; }
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<StudentCheckInOut> CheckInsOuts { get; set; } = new List<StudentCheckInOut>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

public class ClassRoom : BaseEntity
{
    public string ClassName { get; set; } = string.Empty;
    public string AgeGroup { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public Guid? TeacherId { get; set; }
    public Guid? BranchId { get; set; }

    public Teacher? Teacher { get; set; }
    public SchoolBranch? Branch { get; set; }
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
    public ICollection<FeeStructure> FeeStructures { get; set; } = new List<FeeStructure>();
}

public class Attendance : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid ClassId { get; set; }
    public DateOnly Date { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Remarks { get; set; }

    public Student? Student { get; set; }
    public ClassRoom? ClassRoom { get; set; }
}

public class StudentCheckInOut : BaseEntity
{
    public Guid StudentId { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string PickupPerson { get; set; } = string.Empty;
    public string PickupRelationship { get; set; } = string.Empty;
    public string? Notes { get; set; }

    public Student? Student { get; set; }
}

public class Timetable : BaseEntity
{
    public Guid ClassId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string ActivityName { get; set; } = string.Empty;
    public Guid TeacherId { get; set; }

    public ClassRoom? ClassRoom { get; set; }
    public Teacher? Teacher { get; set; }
}

public class FeeStructure : BaseEntity
{
    public Guid ClassId { get; set; }
    public string FeeType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateOnly DueDate { get; set; }

    public ClassRoom? ClassRoom { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

public class Payment : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid FeeStructureId { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public Student? Student { get; set; }
    public FeeStructure? FeeStructure { get; set; }
}

public class InventoryItem : BaseEntity
{
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int AvailableQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public string SupplierName { get; set; } = string.Empty;
}

public class Notice : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Author { get; set; } = string.Empty;
    public NoticeTargetAudience TargetAudience { get; set; }
}

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType NotificationType { get; set; }
    public bool IsRead { get; set; }

    public User? User { get; set; }
}

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }

    public User? User { get; set; }
}
