using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PreschoolManagement.Application.Common;
using PreschoolManagement.Domain.Entities;
using PreschoolManagement.Domain.Enums;
using PreschoolManagement.Infrastructure.Identity;
using PreschoolManagement.Infrastructure.Persistence;

namespace PreschoolManagement.API.Controllers;

[ApiController]
[Route("api/test-data")]
[Authorize(Roles = "SuperAdmin")]
public class TestDataController : ControllerBase
{
    private readonly PreschoolDbContext _dbContext;

    public TestDataController(PreschoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetStatusAsync(CancellationToken cancellationToken)
    {
        var result = new
        {
            Roles = await _dbContext.Roles.CountAsync(cancellationToken),
            Users = await _dbContext.Users.CountAsync(cancellationToken),
            Schools = await _dbContext.Schools.CountAsync(cancellationToken),
            Teachers = await _dbContext.Teachers.CountAsync(cancellationToken),
            Parents = await _dbContext.Parents.CountAsync(cancellationToken),
            Students = await _dbContext.Students.CountAsync(cancellationToken),
            Classes = await _dbContext.ClassRooms.CountAsync(cancellationToken),
            Attendance = await _dbContext.Attendances.CountAsync(cancellationToken),
            Payments = await _dbContext.Payments.CountAsync(cancellationToken),
            InventoryItems = await _dbContext.InventoryItems.CountAsync(cancellationToken)
        };

        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPost("seed-quick-qa")]
    public async Task<IActionResult> SeedQuickQaAsync(CancellationToken cancellationToken)
    {
        var roleLookup = await EnsureRolesAsync(cancellationToken);

        var schoolAddress = await _dbContext.Addresses.FirstOrDefaultAsync(
            x => x.AddressLine1 == "21 Sunshine Street" && x.City == "Springfield", cancellationToken);
        if (schoolAddress is null)
        {
            schoolAddress = new Address
            {
                AddressLine1 = "21 Sunshine Street",
                City = "Springfield",
                State = "CA",
                PostalCode = "94105",
                Country = "USA",
                CreatedBy = "qa-seed"
            };
            _dbContext.Addresses.Add(schoolAddress);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var school = await _dbContext.Schools.FirstOrDefaultAsync(x => x.SchoolName == "Happy Kids Preschool", cancellationToken);
        if (school is null)
        {
            school = new School
            {
                SchoolName = "Happy Kids Preschool",
                AddressId = schoolAddress.Id,
                ContactNumber = "1112223333",
                Email = "info@happykids.local",
                CreatedBy = "qa-seed"
            };
            _dbContext.Schools.Add(school);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        if (school.AddressId is null)
        {
            school.AddressId = schoolAddress.Id;
            _dbContext.Schools.Update(school);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var branch = await _dbContext.SchoolBranches.FirstOrDefaultAsync(
            x => x.SchoolId == school.Id && x.BranchCode == "HKP-MAIN", cancellationToken);
        if (branch is null)
        {
            branch = new SchoolBranch
            {
                SchoolId = school.Id,
                AddressId = schoolAddress.Id,
                BranchName = "Happy Kids - Main Branch",
                BranchCode = "HKP-MAIN",
                ContactNumber = "1112223333",
                Email = "main@happykids.local",
                CreatedBy = "qa-seed"
            };
            _dbContext.SchoolBranches.Add(branch);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var teacherUser = await EnsureUserAsync("teacher.qa@preschool.local", "Teacher", "Demo", roleLookup[UserRole.Teacher], cancellationToken);
        var parentUser = await EnsureUserAsync("parent.qa@preschool.local", "Parent", "Demo", roleLookup[UserRole.Parent], cancellationToken);
        var adminUser = await EnsureUserAsync("admin.qa@preschool.local", "Admin", "Demo", roleLookup[UserRole.PreschoolAdmin], cancellationToken);

        var teacher = await _dbContext.Teachers.FirstOrDefaultAsync(x => x.Email == teacherUser.Email, cancellationToken);
        if (teacher is null)
        {
            var teacherAddress = new Address
            {
                AddressLine1 = "89 Maple Drive",
                City = "Springfield",
                State = "CA",
                PostalCode = "94107",
                Country = "USA",
                CreatedBy = "qa-seed"
            };
            _dbContext.Addresses.Add(teacherAddress);
            await _dbContext.SaveChangesAsync(cancellationToken);

            teacher = new Teacher
            {
                EmployeeCode = "TCH-001",
                FirstName = "Emma",
                LastName = "Watson",
                Gender = Gender.Female,
                DOB = new DateTime(1992, 5, 1),
                Qualification = "B.Ed",
                Experience = 6,
                PhoneNumber = "9990001111",
                Email = teacherUser.Email,
                JoiningDate = DateTime.UtcNow.AddYears(-2),
                SchoolId = school.Id,
                BranchId = branch.Id,
                AddressId = teacherAddress.Id,
                CreatedBy = "qa-seed"
            };
            _dbContext.Teachers.Add(teacher);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var classRoom = await _dbContext.ClassRooms.FirstOrDefaultAsync(x => x.ClassName == "Nursery A", cancellationToken);
        if (classRoom is null)
        {
            classRoom = new ClassRoom
            {
                ClassName = "Nursery A",
                AgeGroup = "3-4",
                Capacity = 25,
                TeacherId = teacher.Id,
                BranchId = branch.Id,
                CreatedBy = "qa-seed"
            };
            _dbContext.ClassRooms.Add(classRoom);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var parent = await _dbContext.Parents.FirstOrDefaultAsync(x => x.UserId == parentUser.Id, cancellationToken);
        if (parent is null)
        {
            parent = new Parent
            {
                FatherName = "Noah Demo",
                MotherName = "Olivia Demo",
                Email = parentUser.Email,
                PhoneNumber = "8887776666",
                Address = "34 Blossom Avenue",
                Occupation = "Engineer",
                UserId = parentUser.Id,
                CreatedBy = "qa-seed"
            };
            _dbContext.Parents.Add(parent);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var student = await _dbContext.Students.FirstOrDefaultAsync(x => x.AdmissionNumber == "ADM-0001", cancellationToken);
        if (student is null)
        {
            var studentAddress = new Address
            {
                AddressLine1 = "34 Blossom Avenue",
                City = "Springfield",
                State = "CA",
                PostalCode = "94108",
                Country = "USA",
                CreatedBy = "qa-seed"
            };
            _dbContext.Addresses.Add(studentAddress);
            await _dbContext.SaveChangesAsync(cancellationToken);

            student = new Student
            {
                AdmissionNumber = "ADM-0001",
                FirstName = "Liam",
                LastName = "Demo",
                Gender = Gender.Male,
                DOB = new DateTime(2021, 4, 10),
                BloodGroup = "O+",
                AddressId = studentAddress.Id,
                JoiningDate = DateTime.UtcNow.AddMonths(-6),
                ClassId = classRoom.Id,
                ParentId = parent.Id,
                BranchId = branch.Id,
                Status = StudentStatus.Active,
                CreatedBy = "qa-seed"
            };
            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var feeStructure = await _dbContext.FeeStructures.FirstOrDefaultAsync(x => x.ClassId == classRoom.Id && x.FeeType == "Monthly Tuition", cancellationToken);
        if (feeStructure is null)
        {
            feeStructure = new FeeStructure
            {
                ClassId = classRoom.Id,
                FeeType = "Monthly Tuition",
                Amount = 2500,
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)),
                CreatedBy = "qa-seed"
            };
            _dbContext.FeeStructures.Add(feeStructure);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        if (!await _dbContext.Payments.AnyAsync(x => x.TransactionReference == "TXN-QA-0001", cancellationToken))
        {
            _dbContext.Payments.Add(new Payment
            {
                StudentId = student.Id,
                FeeStructureId = feeStructure.Id,
                AmountPaid = 2500,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = PaymentMethod.BankTransfer,
                TransactionReference = "TXN-QA-0001",
                Status = PaymentStatus.Paid,
                CreatedBy = "qa-seed"
            });
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (!await _dbContext.Attendances.AnyAsync(x => x.StudentId == student.Id && x.Date == today, cancellationToken))
        {
            _dbContext.Attendances.Add(new Attendance
            {
                StudentId = student.Id,
                ClassId = classRoom.Id,
                Date = today,
                Status = AttendanceStatus.Present,
                Remarks = "On time",
                CreatedBy = "qa-seed"
            });
        }

        if (!await _dbContext.CheckInOuts.AnyAsync(x => x.StudentId == student.Id && x.CheckInTime.Date == DateTime.UtcNow.Date, cancellationToken))
        {
            _dbContext.CheckInOuts.Add(new StudentCheckInOut
            {
                StudentId = student.Id,
                CheckInTime = DateTime.UtcNow.AddHours(-3),
                CheckOutTime = DateTime.UtcNow,
                PickupPerson = "Noah Demo",
                PickupRelationship = "Father",
                Notes = "Verified by QR",
                CreatedBy = "qa-seed"
            });
        }

        if (!await _dbContext.Timetables.AnyAsync(x => x.ClassId == classRoom.Id && x.ActivityName == "Story Time", cancellationToken))
        {
            _dbContext.Timetables.Add(new Timetable
            {
                ClassId = classRoom.Id,
                DayOfWeek = DayOfWeek.Monday,
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(9, 45),
                ActivityName = "Story Time",
                TeacherId = teacher.Id,
                CreatedBy = "qa-seed"
            });
        }

        if (!await _dbContext.InventoryItems.AnyAsync(x => x.ItemName == "Crayon Set", cancellationToken))
        {
            _dbContext.InventoryItems.Add(new InventoryItem
            {
                ItemName = "Crayon Set",
                Category = "Stationery",
                Quantity = 120,
                AvailableQuantity = 100,
                UnitPrice = 3.50m,
                PurchaseDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30)),
                SupplierName = "Little Supplies Ltd",
                CreatedBy = "qa-seed"
            });
        }

        if (!await _dbContext.Notices.AnyAsync(x => x.Title == "PTM Schedule", cancellationToken))
        {
            _dbContext.Notices.Add(new Notice
            {
                Title = "PTM Schedule",
                Description = "Parent-teacher meeting on Friday at 4 PM.",
                PublishDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(15),
                Author = "Admin",
                TargetAudience = NoticeTargetAudience.All,
                CreatedBy = "qa-seed"
            });
        }

        if (!await _dbContext.Notifications.AnyAsync(x => x.UserId == parentUser.Id && x.Title == "Attendance Alert", cancellationToken))
        {
            _dbContext.Notifications.Add(new Notification
            {
                UserId = parentUser.Id,
                Title = "Attendance Alert",
                Message = "Liam marked Present today.",
                NotificationType = NotificationType.AttendanceAlert,
                IsRead = false,
                CreatedBy = "qa-seed"
            });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        var payload = new
        {
            Message = "QA seed completed",
            Credentials = new[]
            {
                new { Email = "superadmin@preschool.local", Password = "Admin@123", Role = "SuperAdmin" },
                new { Email = "admin.qa@preschool.local", Password = "Admin@123", Role = "PreschoolAdmin" },
                new { Email = "teacher.qa@preschool.local", Password = "Admin@123", Role = "Teacher" },
                new { Email = "parent.qa@preschool.local", Password = "Admin@123", Role = "Parent" }
            },
            Entities = new
            {
                SchoolId = school.Id,
                TeacherId = teacher.Id,
                ClassId = classRoom.Id,
                ParentId = parent.Id,
                StudentId = student.Id
            }
        };

        return Ok(ApiResponse<object>.Ok(payload, "Test data seeded"));
    }

    private async Task<Dictionary<UserRole, Guid>> EnsureRolesAsync(CancellationToken cancellationToken)
    {
        var map = new Dictionary<UserRole, Guid>();
        foreach (var role in new[] { UserRole.SuperAdmin, UserRole.PreschoolAdmin, UserRole.Teacher, UserRole.Parent })
        {
            var roleName = role.ToString();
            var existing = await _dbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == roleName, cancellationToken);
            if (existing is null)
            {
                existing = new Role
                {
                    RoleName = roleName,
                    CreatedBy = "qa-seed"
                };
                _dbContext.Roles.Add(existing);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            map[role] = existing.Id;
        }

        return map;
    }

    private async Task<User> EnsureUserAsync(string email, string firstName, string lastName, Guid roleId, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        if (existing is not null)
        {
            return existing;
        }

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = "0000000000",
            RoleId = roleId,
            IsActive = true,
            PasswordHash = PasswordHasherUtility.HashPassword("Admin@123"),
            CreatedBy = "qa-seed"
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }
}
