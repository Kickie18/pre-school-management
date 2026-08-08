using Microsoft.EntityFrameworkCore;
using PreschoolManagement.Application.DTOs;
using PreschoolManagement.Application.Interfaces;
using PreschoolManagement.Infrastructure.Persistence;

namespace PreschoolManagement.Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly PreschoolDbContext _dbContext;

    public ReportService(PreschoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<object> AttendanceReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Attendances.AsNoTracking().AsQueryable();
        query = ApplyDateRange(query, filter);
        if (filter.ClassId.HasValue)
        {
            query = query.Where(x => x.ClassId == filter.ClassId.Value);
        }

        return await query.GroupBy(x => x.Status)
            .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
            .ToListAsync(cancellationToken);
    }

    public async Task<object> StudentReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Students.AsNoTracking().AsQueryable();
        if (filter.ClassId.HasValue)
        {
            query = query.Where(x => x.ClassId == filter.ClassId.Value);
        }

        return await query.Select(x => new
        {
            x.Id,
            x.AdmissionNumber,
            FullName = x.FirstName + " " + x.LastName,
            x.Status,
            x.ClassId
        }).ToListAsync(cancellationToken);
    }

    public async Task<object> FeeCollectionReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Payments.AsNoTracking().AsQueryable();
        if (filter.FromDate.HasValue)
        {
            query = query.Where(x => x.PaymentDate >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            query = query.Where(x => x.PaymentDate <= filter.ToDate.Value);
        }

        return await query.GroupBy(x => x.PaymentDate.Month)
            .Select(g => new { Month = g.Key, Amount = g.Sum(x => x.AmountPaid) })
            .OrderBy(x => x.Month)
            .ToListAsync(cancellationToken);
    }

    public async Task<object> PaymentDueReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default)
    {
        var due = await (from fs in _dbContext.FeeStructures.AsNoTracking()
                         join c in _dbContext.ClassRooms.AsNoTracking() on fs.ClassId equals c.Id
                         where fs.DueDate <= DateOnly.FromDateTime(DateTime.UtcNow)
                         select new
                         {
                             fs.Id,
                             fs.FeeType,
                             fs.Amount,
                             fs.DueDate,
                             ClassName = c.ClassName
                         }).ToListAsync(cancellationToken);
        return due;
    }

    public async Task<object> TeacherReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Teachers.AsNoTracking()
            .Select(x => new
            {
                x.Id,
                x.EmployeeCode,
                FullName = x.FirstName + " " + x.LastName,
                x.Experience,
                x.SchoolId
            }).ToListAsync(cancellationToken);
    }

    public async Task<object> InventoryReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default)
    {
        return await _dbContext.InventoryItems.AsNoTracking()
            .Select(x => new
            {
                x.Id,
                x.ItemName,
                x.Category,
                x.AvailableQuantity,
                IsLowStock = x.AvailableQuantity <= 5,
                x.UnitPrice
            }).ToListAsync(cancellationToken);
    }

    public async Task<object> DailyCheckInOutReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default)
    {
        var day = filter.FromDate?.Date ?? DateTime.UtcNow.Date;
        return await _dbContext.CheckInOuts.AsNoTracking()
            .Where(x => x.CheckInTime.Date == day)
            .Select(x => new
            {
                x.StudentId,
                x.CheckInTime,
                x.CheckOutTime,
                x.PickupPerson,
                x.PickupRelationship
            }).ToListAsync(cancellationToken);
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        return new DashboardSummaryDto
        {
            TotalStudents = await _dbContext.Students.CountAsync(cancellationToken),
            TotalTeachers = await _dbContext.Teachers.CountAsync(cancellationToken),
            TotalClasses = await _dbContext.ClassRooms.CountAsync(cancellationToken),
            TotalParents = await _dbContext.Parents.CountAsync(cancellationToken),
            TodayAttendanceCount = await _dbContext.Attendances.CountAsync(x => x.Date == today, cancellationToken),
            MonthlyRevenue = await _dbContext.Payments
                .Where(x => x.PaymentDate >= monthStart)
                .SumAsync(x => (decimal?)x.AmountPaid, cancellationToken) ?? 0
        };
    }

    private static IQueryable<Domain.Entities.Attendance> ApplyDateRange(IQueryable<Domain.Entities.Attendance> query, ReportFilterDto filter)
    {
        if (filter.FromDate.HasValue)
        {
            var from = DateOnly.FromDateTime(filter.FromDate.Value);
            query = query.Where(x => x.Date >= from);
        }

        if (filter.ToDate.HasValue)
        {
            var to = DateOnly.FromDateTime(filter.ToDate.Value);
            query = query.Where(x => x.Date <= to);
        }

        if (filter.StudentId.HasValue)
        {
            query = query.Where(x => x.StudentId == filter.StudentId.Value);
        }

        return query;
    }
}
