using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;
using TeamHub.Domain.Entities;

namespace TeamHub.Application.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<UserModel>> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        var userList = new List<UserModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userList.Add(new UserModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Roles = roles.ToList()
            });
        }
        return userList;
    }

    public async Task<UserModel> GetUserById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);
        return new UserModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Roles = roles.ToList()
        };
    }

    public async Task<UserModel> CreateEmployee(UserModel model)
    {
        if (await _userManager.FindByEmailAsync(model.Email) != null)
            return null;

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return null;

        await _userManager.AddToRoleAsync(user, "Employee");

        return new UserModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Roles = new List<string> { "Employee" }
        };
    }

    public async Task<UserModel> UpdateUser(string userId, UserModel model)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        user.FullName = model.FullName;
        user.Email = model.Email;
        await _userManager.UpdateAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        return new UserModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Roles = roles.ToList()
        };
    }

    public async Task<bool> DeleteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        await _userManager.DeleteAsync(user);
        return true;
    }
}
