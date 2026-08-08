using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PreschoolManagement.Domain.Common;
using PreschoolManagement.Domain.Entities;

namespace PreschoolManagement.Infrastructure.Persistence;

public class PreschoolDbContext : DbContext
{
    public PreschoolDbContext(DbContextOptions<PreschoolDbContext> options) : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<School> Schools => Set<School>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Parent> Parents => Set<Parent>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<ClassRoom> ClassRooms => Set<ClassRoom>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<StudentCheckInOut> CheckInOuts => Set<StudentCheckInOut>();
    public DbSet<Timetable> Timetables => Set<Timetable>();
    public DbSet<FeeStructure> FeeStructures => Set<FeeStructure>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<Notice> Notices => Set<Notice>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ApplySoftDeleteQueryFilters(modelBuilder);

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(x => x.RoleName).IsRequired().HasMaxLength(50);
            entity.HasIndex(x => x.RoleName).IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(256);
            entity.Property(x => x.PhoneNumber).HasMaxLength(20);
            entity.Property(x => x.PasswordHash).IsRequired();
            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasOne(x => x.Role).WithMany(x => x.Users).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.Property(x => x.SchoolName).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Address).IsRequired().HasMaxLength(300);
            entity.Property(x => x.ContactNumber).HasMaxLength(20);
            entity.Property(x => x.Email).HasMaxLength(256);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.Property(x => x.EmployeeCode).IsRequired().HasMaxLength(30);
            entity.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(x => x.EmployeeCode).IsUnique();
            entity.HasOne(x => x.School).WithMany(x => x.Teachers).HasForeignKey(x => x.SchoolId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Parent>(entity =>
        {
            entity.Property(x => x.FatherName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.MotherName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(256);
            entity.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(x => x.Address).IsRequired().HasMaxLength(300);
            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasOne(x => x.User).WithOne(x => x.Parent).HasForeignKey<Parent>(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ClassRoom>(entity =>
        {
            entity.Property(x => x.ClassName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.AgeGroup).IsRequired().HasMaxLength(50);
            entity.HasIndex(x => x.ClassName).IsUnique();
            entity.HasOne(x => x.Teacher).WithMany(x => x.Classes).HasForeignKey(x => x.TeacherId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(x => x.AdmissionNumber).IsRequired().HasMaxLength(50);
            entity.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.BloodGroup).HasMaxLength(10);
            entity.Property(x => x.Address).HasMaxLength(300);
            entity.HasIndex(x => x.AdmissionNumber).IsUnique();
            entity.HasOne(x => x.Parent).WithMany(x => x.Students).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.ClassRoom).WithMany(x => x.Students).HasForeignKey(x => x.ClassId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasIndex(x => new { x.StudentId, x.Date }).IsUnique();
            entity.HasOne(x => x.Student).WithMany(x => x.Attendances).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.ClassRoom).WithMany(x => x.Attendances).HasForeignKey(x => x.ClassId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StudentCheckInOut>(entity =>
        {
            entity.Property(x => x.PickupPerson).IsRequired().HasMaxLength(100);
            entity.Property(x => x.PickupRelationship).IsRequired().HasMaxLength(100);
            entity.HasOne(x => x.Student).WithMany(x => x.CheckInsOuts).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Timetable>(entity =>
        {
            entity.Property(x => x.ActivityName).IsRequired().HasMaxLength(200);
            entity.HasOne(x => x.ClassRoom).WithMany(x => x.Timetables).HasForeignKey(x => x.ClassId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Teacher).WithMany(x => x.Timetables).HasForeignKey(x => x.TeacherId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FeeStructure>(entity =>
        {
            entity.Property(x => x.FeeType).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Amount).HasPrecision(18, 2);
            entity.HasOne(x => x.ClassRoom).WithMany(x => x.FeeStructures).HasForeignKey(x => x.ClassId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(x => x.AmountPaid).HasPrecision(18, 2);
            entity.Property(x => x.TransactionReference).IsRequired().HasMaxLength(100);
            entity.HasIndex(x => x.TransactionReference).IsUnique();
            entity.HasOne(x => x.Student).WithMany(x => x.Payments).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.FeeStructure).WithMany(x => x.Payments).HasForeignKey(x => x.FeeStructureId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.Property(x => x.ItemName).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Category).IsRequired().HasMaxLength(100);
            entity.Property(x => x.SupplierName).HasMaxLength(200);
            entity.Property(x => x.UnitPrice).HasPrecision(18, 2);
            entity.HasIndex(x => x.ItemName);
        });

        modelBuilder.Entity<Notice>(entity =>
        {
            entity.Property(x => x.Title).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Description).IsRequired().HasMaxLength(2000);
            entity.Property(x => x.Author).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(x => x.Title).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Message).IsRequired().HasMaxLength(2000);
            entity.HasOne(x => x.User).WithMany(x => x.Notifications).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.Property(x => x.Token).IsRequired().HasMaxLength(256);
            entity.HasIndex(x => x.Token).IsUnique();
            entity.HasOne(x => x.User).WithMany(x => x.RefreshTokens).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedDate = DateTime.UtcNow;
            }
        }
    }

    private static void ApplySoftDeleteQueryFilters(ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model.GetEntityTypes()
            .Where(x => typeof(BaseEntity).IsAssignableFrom(x.ClrType));

        foreach (var entityType in entities)
        {
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var isDeletedProperty = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var compareExpression = Expression.Equal(isDeletedProperty, Expression.Constant(false));
            var lambda = Expression.Lambda(compareExpression, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
}
