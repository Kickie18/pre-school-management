using PreschoolManagement.Domain.Entities;

namespace PreschoolManagement.Application.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<Role> Roles { get; }
    IGenericRepository<User> Users { get; }
    IGenericRepository<School> Schools { get; }
    IGenericRepository<Teacher> Teachers { get; }
    IGenericRepository<Parent> Parents { get; }
    IGenericRepository<Student> Students { get; }
    IGenericRepository<ClassRoom> ClassRooms { get; }
    IGenericRepository<Attendance> Attendances { get; }
    IGenericRepository<StudentCheckInOut> CheckInOuts { get; }
    IGenericRepository<Timetable> Timetables { get; }
    IGenericRepository<FeeStructure> FeeStructures { get; }
    IGenericRepository<Payment> Payments { get; }
    IGenericRepository<InventoryItem> InventoryItems { get; }
    IGenericRepository<Notice> Notices { get; }
    IGenericRepository<Notification> Notifications { get; }
    IGenericRepository<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
