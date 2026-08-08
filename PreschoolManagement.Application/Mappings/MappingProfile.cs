using AutoMapper;
using PreschoolManagement.Application.DTOs;
using PreschoolManagement.Domain.Entities;

namespace PreschoolManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<RoleCreateDto, Role>();
        CreateMap<RoleUpdateDto, Role>();

        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();

        CreateMap<School, SchoolDto>().ReverseMap();
        CreateMap<SchoolCreateDto, School>();
        CreateMap<SchoolUpdateDto, School>();

        CreateMap<Teacher, TeacherDto>().ReverseMap();
        CreateMap<TeacherCreateDto, Teacher>();
        CreateMap<TeacherUpdateDto, Teacher>();

        CreateMap<Parent, ParentDto>().ReverseMap();
        CreateMap<ParentCreateDto, Parent>();
        CreateMap<ParentUpdateDto, Parent>();

        CreateMap<Student, StudentDto>().ReverseMap();
        CreateMap<StudentCreateDto, Student>();
        CreateMap<StudentUpdateDto, Student>();

        CreateMap<ClassRoom, ClassRoomDto>().ReverseMap();
        CreateMap<ClassRoomCreateDto, ClassRoom>();
        CreateMap<ClassRoomUpdateDto, ClassRoom>();

        CreateMap<Attendance, AttendanceDto>().ReverseMap();
        CreateMap<AttendanceCreateDto, Attendance>();
        CreateMap<AttendanceUpdateDto, Attendance>();

        CreateMap<StudentCheckInOut, StudentCheckInOutDto>().ReverseMap();
        CreateMap<StudentCheckInOutCreateDto, StudentCheckInOut>();
        CreateMap<StudentCheckInOutUpdateDto, StudentCheckInOut>();

        CreateMap<Timetable, TimetableDto>().ReverseMap();
        CreateMap<TimetableCreateDto, Timetable>();
        CreateMap<TimetableUpdateDto, Timetable>();

        CreateMap<FeeStructure, FeeStructureDto>().ReverseMap();
        CreateMap<FeeStructureCreateDto, FeeStructure>();
        CreateMap<FeeStructureUpdateDto, FeeStructure>();

        CreateMap<Payment, PaymentDto>().ReverseMap();
        CreateMap<PaymentCreateDto, Payment>();
        CreateMap<PaymentUpdateDto, Payment>();

        CreateMap<InventoryItem, InventoryItemDto>().ReverseMap();
        CreateMap<InventoryItemCreateDto, InventoryItem>();
        CreateMap<InventoryItemUpdateDto, InventoryItem>();

        CreateMap<Notice, NoticeDto>().ReverseMap();
        CreateMap<NoticeCreateDto, Notice>();
        CreateMap<NoticeUpdateDto, Notice>();

        CreateMap<Notification, NotificationDto>().ReverseMap();
        CreateMap<NotificationCreateDto, Notification>();
        CreateMap<NotificationUpdateDto, Notification>();
    }
}
