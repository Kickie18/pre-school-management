using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PreschoolManagement.Application.Common;
using PreschoolManagement.Application.DTOs;
using PreschoolManagement.Application.Interfaces;

namespace PreschoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserService _currentUserService;

    public AuthController(IAuthService authService, ICurrentUserService currentUserService)
    {
        _authService = authService;
        _currentUserService = currentUserService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);
        return Ok(ApiResponse<AuthResponse>.Ok(result, "Registration successful"));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);
        return result is null
            ? Unauthorized(ApiResponse<AuthResponse>.Fail("Invalid credentials"))
            : Ok(ApiResponse<AuthResponse>.Ok(result, "Login successful"));
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RefreshTokenAsync(request, cancellationToken);
        return result is null
            ? Unauthorized(ApiResponse<AuthResponse>.Fail("Invalid refresh token"))
            : Ok(ApiResponse<AuthResponse>.Ok(result));
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var success = await _authService.ForgotPasswordAsync(request, cancellationToken);
        return Ok(ApiResponse<object>.Ok(new { success }, "If email exists, reset instructions have been sent."));
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var success = await _authService.ResetPasswordAsync(request, cancellationToken);
        return success
            ? Ok(ApiResponse<object>.Ok(new { success }, "Password reset successful"))
            : BadRequest(ApiResponse<object>.Fail("Password reset failed"));
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid user context"));
        }

        var success = await _authService.ChangePasswordAsync(_currentUserService.UserId.Value, request, cancellationToken);
        return success
            ? Ok(ApiResponse<object>.Ok(new { success }, "Password changed successfully"))
            : BadRequest(ApiResponse<object>.Fail("Password change failed"));
    }
}
