using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PreschoolManagement.Application.DTOs;
using PreschoolManagement.Application.Interfaces;
using PreschoolManagement.Domain.Entities;
using PreschoolManagement.Infrastructure.Identity;
using PreschoolManagement.Infrastructure.Persistence;
using PreschoolManagement.Infrastructure.Repositories;
using PreschoolManagement.Infrastructure.Services;

namespace PreschoolManagement.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing in configuration.");

        services.AddDbContext<PreschoolDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddScoped<ICrudService<RoleDto, RoleCreateDto, RoleUpdateDto>>(sp =>
            new CrudService<Role, RoleDto, RoleCreateDto, RoleUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.Roles));

        services.AddScoped<ICrudService<UserDto, UserCreateDto, UserUpdateDto>>(sp =>
            new CrudService<User, UserDto, UserCreateDto, UserUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.Users));

        services.AddScoped<ICrudService<SchoolDto, SchoolCreateDto, SchoolUpdateDto>>(sp =>
            new CrudService<School, SchoolDto, SchoolCreateDto, SchoolUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.Schools));

        services.AddScoped<ICrudService<TeacherDto, TeacherCreateDto, TeacherUpdateDto>>(sp =>
            new CrudService<Teacher, TeacherDto, TeacherCreateDto, TeacherUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.Teachers));

        services.AddScoped<ICrudService<ParentDto, ParentCreateDto, ParentUpdateDto>>(sp =>
            new CrudService<Parent, ParentDto, ParentCreateDto, ParentUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.Parents));

        services.AddScoped<ICrudService<StudentDto, StudentCreateDto, StudentUpdateDto>>(sp =>
            new CrudService<Student, StudentDto, StudentCreateDto, StudentUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.Students));

        services.AddScoped<ICrudService<ClassRoomDto, ClassRoomCreateDto, ClassRoomUpdateDto>>(sp =>
            new CrudService<ClassRoom, ClassRoomDto, ClassRoomCreateDto, ClassRoomUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.ClassRooms));

        services.AddScoped<ICrudService<AttendanceDto, AttendanceCreateDto, AttendanceUpdateDto>>(sp =>
            new CrudService<Attendance, AttendanceDto, AttendanceCreateDto, AttendanceUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.Attendances));

        services.AddScoped<ICrudService<StudentCheckInOutDto, StudentCheckInOutCreateDto, StudentCheckInOutUpdateDto>>(sp =>
            new CrudService<StudentCheckInOut, StudentCheckInOutDto, StudentCheckInOutCreateDto, StudentCheckInOutUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.CheckInOuts));

        services.AddScoped<ICrudService<TimetableDto, TimetableCreateDto, TimetableUpdateDto>>(sp =>
            new CrudService<Timetable, TimetableDto, TimetableCreateDto, TimetableUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.Timetables));

        services.AddScoped<ICrudService<FeeStructureDto, FeeStructureCreateDto, FeeStructureUpdateDto>>(sp =>
            new CrudService<FeeStructure, FeeStructureDto, FeeStructureCreateDto, FeeStructureUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.FeeStructures));

        services.AddScoped<ICrudService<PaymentDto, PaymentCreateDto, PaymentUpdateDto>>(sp =>
            new CrudService<Payment, PaymentDto, PaymentCreateDto, PaymentUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.Payments));

        services.AddScoped<ICrudService<InventoryItemDto, InventoryItemCreateDto, InventoryItemUpdateDto>>(sp =>
            new CrudService<InventoryItem, InventoryItemDto, InventoryItemCreateDto, InventoryItemUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.InventoryItems));

        services.AddScoped<ICrudService<NoticeDto, NoticeCreateDto, NoticeUpdateDto>>(sp =>
            new CrudService<Notice, NoticeDto, NoticeCreateDto, NoticeUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.Notices));

        services.AddScoped<ICrudService<NotificationDto, NotificationCreateDto, NotificationUpdateDto>>(sp =>
            new CrudService<Notification, NotificationDto, NotificationCreateDto, NotificationUpdateDto>(
                sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<IMapper>(), u => u.Notifications));

        return services;
    }
}
