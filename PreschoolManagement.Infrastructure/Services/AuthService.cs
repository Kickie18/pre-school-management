using AutoMapper;
using PreschoolManagement.Application.DTOs;
using PreschoolManagement.Application.Interfaces;
using PreschoolManagement.Domain.Entities;
using PreschoolManagement.Domain.Enums;
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

        var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId, cancellationToken)
            ?? throw new InvalidOperationException("Invalid role selected.");

        var schoolIds = request.SchoolIds
            .Where(x => x != Guid.Empty)
            .Distinct()
            .ToList();

        var isPreschoolAdmin = string.Equals(role.RoleName, UserRole.PreschoolAdmin.ToString(), StringComparison.OrdinalIgnoreCase);
        if (isPreschoolAdmin && schoolIds.Count == 0)
        {
            throw new InvalidOperationException("At least one school is required for PreschoolAdmin users.");
        }

        if (schoolIds.Count > 0)
        {
            var schools = await _unitOfWork.Schools.FindAsync(x => schoolIds.Contains(x.Id), cancellationToken);
            var foundSchoolIds = schools.Select(x => x.Id).ToHashSet();
            if (schoolIds.Any(x => !foundSchoolIds.Contains(x)))
            {
                throw new InvalidOperationException("One or more provided schools are invalid.");
            }
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

        var roleName = role?.RoleName ?? "Parent";

        foreach (var schoolId in schoolIds)
        {
            await _unitOfWork.UserSchools.AddAsync(new UserSchool
            {
                UserId = user.Id,
                SchoolId = schoolId
            }, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var userDto = _mapper.Map<UserDto>(user);
        userDto.SchoolIds = schoolIds;

        var accessToken = _jwtTokenService.GenerateAccessToken(userDto, roleName, schoolIds);
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
            User = userDto
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

        var schoolIds = (await _unitOfWork.UserSchools.FindAsync(x => x.UserId == user.Id, cancellationToken))
            .Select(x => x.SchoolId)
            .Distinct()
            .ToList();

        var userDto = _mapper.Map<UserDto>(user);
        userDto.SchoolIds = schoolIds;

        var accessToken = _jwtTokenService.GenerateAccessToken(userDto, roleName, schoolIds);
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
            User = userDto
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

        var schoolIds = (await _unitOfWork.UserSchools.FindAsync(x => x.UserId == user.Id, cancellationToken))
            .Select(x => x.SchoolId)
            .Distinct()
            .ToList();

        var userDto = _mapper.Map<UserDto>(user);
        userDto.SchoolIds = schoolIds;

        var accessToken = _jwtTokenService.GenerateAccessToken(userDto, roleName, schoolIds);
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
            User = userDto
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
