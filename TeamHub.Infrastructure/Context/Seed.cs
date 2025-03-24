using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TeamHub.Domain.Entities;
using TeamHub.Infrastructure.Data.Settings;

namespace TeamHub.Infrastructure.Data.Context;

/// <summary>
/// Seeds default roles and the initial admin user into the system.
/// </summary>
public static class Seed
{
    /// <summary>
    /// Seeds predefined roles and a default administrator user.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve required services.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var adminSettings = serviceProvider.GetRequiredService<IOptions<AuthSettings>>().Value;

        // Define roles to ensure they exist
        var roles = new[] { "Administrator", "Employee" };

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
