using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TeamHub.API.Entities;
using TeamHub.API.Models;
using TeamHub.Application.Interfaces;

namespace TeamHub.Application.Services;
public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserModel> GetProfile(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        return new UserModel
        {
            FullName = user.FullName,
            Email = user.Email,
            VirtualPath = user.ProfilePicture
        };
    }

    public async Task<bool> UpdateProfile(string userId, UserModel model)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        user.FullName = model.FullName;
        user.Email = model.Email;
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}
