using PreschoolManagement.Application.Interfaces;
using PreschoolManagement.Domain.Entities;
using PreschoolManagement.Infrastructure.Persistence;

namespace PreschoolManagement.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly PreschoolDbContext _dbContext;

    public UnitOfWork(PreschoolDbContext dbContext)
    {
        _dbContext = dbContext;
        Roles = new GenericRepository<Role>(_dbContext);
        Users = new GenericRepository<User>(_dbContext);
        Schools = new GenericRepository<School>(_dbContext);
        SchoolBranches = new GenericRepository<SchoolBranch>(_dbContext);
        Addresses = new GenericRepository<Address>(_dbContext);
        UserSchools = new GenericRepository<UserSchool>(_dbContext);
        Teachers = new GenericRepository<Teacher>(_dbContext);
        Parents = new GenericRepository<Parent>(_dbContext);
        Students = new GenericRepository<Student>(_dbContext);
        ClassRooms = new GenericRepository<ClassRoom>(_dbContext);
        Attendances = new GenericRepository<Attendance>(_dbContext);
        CheckInOuts = new GenericRepository<StudentCheckInOut>(_dbContext);
        Timetables = new GenericRepository<Timetable>(_dbContext);
        FeeStructures = new GenericRepository<FeeStructure>(_dbContext);
        Payments = new GenericRepository<Payment>(_dbContext);
        InventoryItems = new GenericRepository<InventoryItem>(_dbContext);
        Notices = new GenericRepository<Notice>(_dbContext);
        Notifications = new GenericRepository<Notification>(_dbContext);
        RefreshTokens = new GenericRepository<RefreshToken>(_dbContext);
    }

    public IGenericRepository<Role> Roles { get; }
    public IGenericRepository<User> Users { get; }
    public IGenericRepository<School> Schools { get; }
    public IGenericRepository<SchoolBranch> SchoolBranches { get; }
    public IGenericRepository<Address> Addresses { get; }
    public IGenericRepository<UserSchool> UserSchools { get; }
    public IGenericRepository<Teacher> Teachers { get; }
    public IGenericRepository<Parent> Parents { get; }
    public IGenericRepository<Student> Students { get; }
    public IGenericRepository<ClassRoom> ClassRooms { get; }
    public IGenericRepository<Attendance> Attendances { get; }
    public IGenericRepository<StudentCheckInOut> CheckInOuts { get; }
    public IGenericRepository<Timetable> Timetables { get; }
    public IGenericRepository<FeeStructure> FeeStructures { get; }
    public IGenericRepository<Payment> Payments { get; }
    public IGenericRepository<InventoryItem> InventoryItems { get; }
    public IGenericRepository<Notice> Notices { get; }
    public IGenericRepository<Notification> Notifications { get; }
    public IGenericRepository<RefreshToken> RefreshTokens { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
