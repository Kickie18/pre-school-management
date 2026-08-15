using PreschoolManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PreschoolManagement.Application.DTOs;

public abstract class BaseDto
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class AddressDto : BaseDto
{
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class AddressCreateDto
{
    [Required]
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    [Required]
    public string City { get; set; } = string.Empty;
    [Required]
    public string State { get; set; } = string.Empty;
    [Required]
    public string PostalCode { get; set; } = string.Empty;
    [Required]
    public string Country { get; set; } = string.Empty;
}

public class RoleDto : BaseDto
{
    public string RoleName { get; set; } = string.Empty;
}

public class RoleCreateDto
{
    public string RoleName { get; set; } = string.Empty;
}

public class RoleUpdateDto : RoleCreateDto
{
}

public class UserDto : BaseDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public string? ProfilePicture { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginDate { get; set; }
}

public class UserCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public string? ProfilePicture { get; set; }
}

public class UserUpdateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
    public bool IsActive { get; set; }
}

public class SchoolDto : BaseDto
{
    public string SchoolName { get; set; } = string.Empty;
    public Guid AddressId { get; set; }
    public AddressDto? Address { get; set; }
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Logo { get; set; }
}

public class SchoolCreateDto
{
    public string SchoolName { get; set; } = string.Empty;
    [Required]
    public AddressCreateDto Address { get; set; } = null!;
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Logo { get; set; }
}

public class SchoolUpdateDto : SchoolCreateDto
{
}

public class TeacherDto : BaseDto
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
    public Guid AddressId { get; set; }
    public Guid UserId { get; set; }
    public AddressDto? Address { get; set; }
}

public class TeacherCreateDto
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
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public AddressCreateDto Address { get; set; } = null!;
}

public class TeacherUpdateDto : TeacherCreateDto
{
}

public class ParentDto : BaseDto
{
    public string FatherName { get; set; } = string.Empty;
    public string MotherName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid SchoolId { get; set; }
}

public class ParentCreateDto
{
    public string FatherName { get; set; } = string.Empty;
    public string MotherName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid SchoolId { get; set; }
}

public class ParentUpdateDto : ParentCreateDto
{
}

public class StudentDto : BaseDto
{
    public string AdmissionNumber { get; set; } = string.Empty;
    public string? RollNumber { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime DOB { get; set; }
    public string BloodGroup { get; set; } = string.Empty;
    public Guid AddressId { get; set; }
    public AddressDto? Address { get; set; }
    public DateTime JoiningDate { get; set; }
    public Guid ClassId { get; set; }
    public Guid ParentId { get; set; }
    public Guid SchoolId { get; set; }
    public string? ProfilePicture { get; set; }
    public StudentStatus Status { get; set; }
}

public class StudentCreateDto
{
    public string AdmissionNumber { get; set; } = string.Empty;
    public string? RollNumber { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime DOB { get; set; }
    public string BloodGroup { get; set; } = string.Empty;
    [Required]
    public AddressCreateDto Address { get; set; } = null!;
    public DateTime JoiningDate { get; set; }
    public Guid ClassId { get; set; }
    public Guid ParentId { get; set; }
    [Required]
    public Guid SchoolId { get; set; }
    public string? ProfilePicture { get; set; }
    public StudentStatus Status { get; set; }
}

public class StudentUpdateDto : StudentCreateDto
{
}

public class ClassRoomDto : BaseDto
{
    public string ClassName { get; set; } = string.Empty;
    public string AgeGroup { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public Guid SchoolId { get; set; }
    public Guid? TeacherId { get; set; }
}

public class ClassRoomCreateDto
{
    public string ClassName { get; set; } = string.Empty;
    public string AgeGroup { get; set; } = string.Empty;
    public int Capacity { get; set; }
    [Required]
    public Guid SchoolId { get; set; }
    public Guid? TeacherId { get; set; }
}

public class ClassRoomUpdateDto : ClassRoomCreateDto
{
}

public class AttendanceDto : BaseDto
{
    public Guid StudentId { get; set; }
    public Guid ClassId { get; set; }
    public DateOnly Date { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Remarks { get; set; }
}

public class AttendanceCreateDto
{
    public Guid StudentId { get; set; }
    public Guid ClassId { get; set; }
    public DateOnly Date { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Remarks { get; set; }
}

public class AttendanceUpdateDto : AttendanceCreateDto
{
}

public class StudentCheckInOutDto : BaseDto
{
    public Guid StudentId { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string PickupPerson { get; set; } = string.Empty;
    public string PickupRelationship { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class StudentCheckInOutCreateDto
{
    public Guid StudentId { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string PickupPerson { get; set; } = string.Empty;
    public string PickupRelationship { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class StudentCheckInOutUpdateDto : StudentCheckInOutCreateDto
{
}

public class TimetableDto : BaseDto
{
    public Guid ClassId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string ActivityName { get; set; } = string.Empty;
    public Guid TeacherId { get; set; }
}

public class TimetableCreateDto
{
    public Guid ClassId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string ActivityName { get; set; } = string.Empty;
    public Guid TeacherId { get; set; }
}

public class TimetableUpdateDto : TimetableCreateDto
{
}

public class FeeStructureDto : BaseDto
{
    public Guid ClassId { get; set; }
    public string FeeType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateOnly DueDate { get; set; }
}

public class FeeStructureCreateDto
{
    public Guid ClassId { get; set; }
    public string FeeType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateOnly DueDate { get; set; }
}

public class FeeStructureUpdateDto : FeeStructureCreateDto
{
}

public class PaymentDto : BaseDto
{
    public Guid StudentId { get; set; }
    public Guid FeeStructureId { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public PaymentStatus Status { get; set; }
}

public class PaymentCreateDto
{
    public Guid StudentId { get; set; }
    public Guid FeeStructureId { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public PaymentStatus Status { get; set; }
}

public class PaymentUpdateDto : PaymentCreateDto
{
}

public class InventoryItemDto : BaseDto
{
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int AvailableQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public string SupplierName { get; set; } = string.Empty;
}

public class InventoryItemCreateDto
{
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int AvailableQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public string SupplierName { get; set; } = string.Empty;
}

public class InventoryItemUpdateDto : InventoryItemCreateDto
{
}

public class NoticeDto : BaseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Author { get; set; } = string.Empty;
    public NoticeTargetAudience TargetAudience { get; set; }
}

public class NoticeCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Author { get; set; } = string.Empty;
    public NoticeTargetAudience TargetAudience { get; set; }
}

public class NoticeUpdateDto : NoticeCreateDto
{
}

public class NotificationDto : BaseDto
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType NotificationType { get; set; }
    public bool IsRead { get; set; }
}

public class NotificationCreateDto
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType NotificationType { get; set; }
    public bool IsRead { get; set; }
}

public class NotificationUpdateDto : NotificationCreateDto
{
}

public class RegisterRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class ForgotPasswordRequest
{
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordRequest
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = new();
}

public class DashboardSummaryDto
{
    public int TotalStudents { get; set; }
    public int TotalTeachers { get; set; }
    public int TotalClasses { get; set; }
    public int TotalParents { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public int TodayAttendanceCount { get; set; }
}

public class ReportFilterDto
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public Guid? ClassId { get; set; }
    public Guid? TeacherId { get; set; }
    public Guid? StudentId { get; set; }
}
