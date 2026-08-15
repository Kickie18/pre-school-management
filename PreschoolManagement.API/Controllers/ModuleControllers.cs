using Microsoft.AspNetCore.Authorization;
using PreschoolManagement.Application.DTOs;
using PreschoolManagement.Application.Interfaces;

namespace PreschoolManagement.API.Controllers;

[Authorize(Roles = "SuperAdmin,PreschoolAdmin")]
public class RolesController : BaseCrudController<RoleDto, RoleCreateDto, RoleUpdateDto>
{
    public RolesController(ICrudService<RoleDto, RoleCreateDto, RoleUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin")]
public class UsersController : BaseCrudController<UserDto, UserCreateDto, UserUpdateDto>
{
    public UsersController(ICrudService<UserDto, UserCreateDto, UserUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin")]
public class SchoolsController : BaseCrudController<SchoolDto, SchoolCreateDto, SchoolUpdateDto>
{
    public SchoolsController(ICrudService<SchoolDto, SchoolCreateDto, SchoolUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin")]
public class TeachersController : BaseCrudController<TeacherDto, TeacherCreateDto, TeacherUpdateDto>
{
    public TeachersController(ICrudService<TeacherDto, TeacherCreateDto, TeacherUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin")]
public class ParentsController : BaseCrudController<ParentDto, ParentCreateDto, ParentUpdateDto>
{
    public ParentsController(ICrudService<ParentDto, ParentCreateDto, ParentUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin,Teacher")]
public class StudentsController : BaseCrudController<StudentDto, StudentCreateDto, StudentUpdateDto>
{
    public StudentsController(ICrudService<StudentDto, StudentCreateDto, StudentUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin,Teacher")]
public class ClassRoomsController : BaseCrudController<ClassRoomDto, ClassRoomCreateDto, ClassRoomUpdateDto>
{
    public ClassRoomsController(ICrudService<ClassRoomDto, ClassRoomCreateDto, ClassRoomUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin,Teacher")]
public class AttendancesController : BaseCrudController<AttendanceDto, AttendanceCreateDto, AttendanceUpdateDto>
{
    public AttendancesController(ICrudService<AttendanceDto, AttendanceCreateDto, AttendanceUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin,Teacher,Parent")]
public class CheckInOutsController : BaseCrudController<StudentCheckInOutDto, StudentCheckInOutCreateDto, StudentCheckInOutUpdateDto>
{
    public CheckInOutsController(ICrudService<StudentCheckInOutDto, StudentCheckInOutCreateDto, StudentCheckInOutUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin,Teacher")]
public class TimetablesController : BaseCrudController<TimetableDto, TimetableCreateDto, TimetableUpdateDto>
{
    public TimetablesController(ICrudService<TimetableDto, TimetableCreateDto, TimetableUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin")]
public class FeeStructuresController : BaseCrudController<FeeStructureDto, FeeStructureCreateDto, FeeStructureUpdateDto>
{
    public FeeStructuresController(ICrudService<FeeStructureDto, FeeStructureCreateDto, FeeStructureUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin")]
public class PaymentsController : BaseCrudController<PaymentDto, PaymentCreateDto, PaymentUpdateDto>
{
    public PaymentsController(ICrudService<PaymentDto, PaymentCreateDto, PaymentUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin")]
public class InventoryItemsController : BaseCrudController<InventoryItemDto, InventoryItemCreateDto, InventoryItemUpdateDto>
{
    public InventoryItemsController(ICrudService<InventoryItemDto, InventoryItemCreateDto, InventoryItemUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin")]
public class NoticesController : BaseCrudController<NoticeDto, NoticeCreateDto, NoticeUpdateDto>
{
    public NoticesController(ICrudService<NoticeDto, NoticeCreateDto, NoticeUpdateDto> service) : base(service) { }
}

[Authorize(Roles = "SuperAdmin,PreschoolAdmin,Teacher,Parent")]
public class NotificationsController : BaseCrudController<NotificationDto, NotificationCreateDto, NotificationUpdateDto>
{
    public NotificationsController(ICrudService<NotificationDto, NotificationCreateDto, NotificationUpdateDto> service) : base(service) { }
}
