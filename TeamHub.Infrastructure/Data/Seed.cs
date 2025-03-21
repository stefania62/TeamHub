using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TeamHub.Domain.Entities;
using TeamHub.Infrastructure.Settings;

namespace TeamHub.Infrastructure.Data;
public static class Seed
{
    public static async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var adminSettings = serviceProvider.GetRequiredService<IOptions<AuthSettings>>().Value;

        // Ensure roles exist
        string[] roles = { "Administrator", "Employee" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create Admin User
        var adminUser = await userManager.FindByEmailAsync(adminSettings.Email);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminSettings.Username,
                Email = adminSettings.Email,
                FullName = adminSettings.FullName
            };

            var result = await userManager.CreateAsync(adminUser, adminSettings.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Administrator");
            }
        }
    }
}
