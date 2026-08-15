using PreschoolManagement.Application.Common;
using PreschoolManagement.Application.DTOs;

namespace PreschoolManagement.Application.Interfaces;

public interface ICrudService<TDto, in TCreateDto, in TUpdateDto>
{
    Task<PagedResult<TDto>> GetAllAsync(QueryParameters query, CancellationToken cancellationToken = default);
    Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TDto> CreateAsync(TCreateDto dto, CancellationToken cancellationToken = default);
    Task<TDto?> UpdateAsync(Guid id, TUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest request, CancellationToken cancellationToken = default);
    Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default);
    Task<bool> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);
}

public interface IJwtTokenService
{
    string GenerateAccessToken(UserDto user, string roleName);
    string GenerateRefreshToken();
}

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Email { get; }
    string? Role { get; }
}

public interface IReportService
{
    Task<object> AttendanceReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default);
    Task<object> StudentReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default);
    Task<object> FeeCollectionReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default);
    Task<object> PaymentDueReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default);
    Task<object> TeacherReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default);
    Task<object> InventoryReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default);
    Task<object> DailyCheckInOutReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default);
    Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
}
