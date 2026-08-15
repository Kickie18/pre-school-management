using AutoMapper;
using PreschoolManagement.Application.DTOs;
using PreschoolManagement.Application.Interfaces;
using PreschoolManagement.Domain.Entities;
using PreschoolManagement.Infrastructure.Identity;

namespace PreschoolManagement.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _unitOfWork.Users.FindAsync(x => x.Email == request.Email, cancellationToken);
        if (existing.Any())
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            RoleId = request.RoleId,
            IsActive = true
        };

        user.PasswordHash = PasswordHasherUtility.HashPassword(request.Password);
        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var role = await _unitOfWork.Roles.GetByIdAsync(user.RoleId, cancellationToken);
        var roleName = role?.RoleName ?? "Parent";

        var accessToken = _jwtTokenService.GenerateAccessToken(_mapper.Map<UserDto>(user), roleName);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        await _unitOfWork.RefreshTokens.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        }, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = (await _unitOfWork.Users.FindAsync(x => x.Email == request.Email && x.IsActive, cancellationToken)).FirstOrDefault();
        if (user is null)
        {
            return null;
        }

        var passwordValid = PasswordHasherUtility.VerifyPassword(request.Password, user.PasswordHash);
        if (!passwordValid)
        {
            return null;
        }

        user.LastLoginDate = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);

        var role = await _unitOfWork.Roles.GetByIdAsync(user.RoleId, cancellationToken);
        var roleName = role?.RoleName ?? "Parent";

        var accessToken = _jwtTokenService.GenerateAccessToken(_mapper.Map<UserDto>(user), roleName);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        await _unitOfWork.RefreshTokens.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        }, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var token = (await _unitOfWork.RefreshTokens.FindAsync(x => x.Token == request.RefreshToken && !x.IsRevoked, cancellationToken)).FirstOrDefault();
        if (token is null || token.ExpiresAt <= DateTime.UtcNow)
        {
            return null;
        }

        var user = await _unitOfWork.Users.GetByIdAsync(token.UserId, cancellationToken);
        if (user is null)
        {
            return null;
        }

        token.IsRevoked = true;
        _unitOfWork.RefreshTokens.Update(token);

        var role = await _unitOfWork.Roles.GetByIdAsync(user.RoleId, cancellationToken);
        var roleName = role?.RoleName ?? "Parent";

        var accessToken = _jwtTokenService.GenerateAccessToken(_mapper.Map<UserDto>(user), roleName);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        await _unitOfWork.RefreshTokens.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        }, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return false;
        }

        var passwordValid = PasswordHasherUtility.VerifyPassword(request.CurrentPassword, user.PasswordHash);
        if (!passwordValid)
        {
            return false;
        }

        user.PasswordHash = PasswordHasherUtility.HashPassword(request.NewPassword);
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = (await _unitOfWork.Users.FindAsync(x => x.Email == request.Email, cancellationToken)).FirstOrDefault();
        return user is not null;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = (await _unitOfWork.Users.FindAsync(x => x.Email == request.Email, cancellationToken)).FirstOrDefault();
        if (user is null)
        {
            return false;
        }

        user.PasswordHash = PasswordHasherUtility.HashPassword(request.NewPassword);
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
