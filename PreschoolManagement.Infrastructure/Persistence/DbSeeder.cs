using Microsoft.Extensions.DependencyInjection;
using PreschoolManagement.Domain.Entities;
using PreschoolManagement.Domain.Enums;
using PreschoolManagement.Infrastructure.Identity;

namespace PreschoolManagement.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PreschoolDbContext>();

        if (context.Roles.Any())
        {
            return;
        }

        var roles = new List<Role>
        {
            new() { RoleName = UserRole.SuperAdmin.ToString(), CreatedBy = "system" },
            new() { RoleName = UserRole.PreschoolAdmin.ToString(), CreatedBy = "system" },
            new() { RoleName = UserRole.Teacher.ToString(), CreatedBy = "system" },
            new() { RoleName = UserRole.Parent.ToString(), CreatedBy = "system" }
        };

        context.Roles.AddRange(roles);
        await context.SaveChangesAsync();

        var superAdminRoleId = roles.First(x => x.RoleName == UserRole.SuperAdmin.ToString()).Id;
        var superAdmin = new User
        {
            FirstName = "Super",
            LastName = "Admin",
            Email = "superadmin@preschool.local",
            PhoneNumber = "0000000000",
            RoleId = superAdminRoleId,
            IsActive = true,
            CreatedBy = "system"
        };
        superAdmin.PasswordHash = PasswordHasherUtility.HashPassword("Admin@123");

        context.Users.Add(superAdmin);
        await context.SaveChangesAsync();
    }
}
