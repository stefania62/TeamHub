using TeamHub.API.Models;

namespace TeamHub.API.Interfaces;
public interface IAdminService
{
    Task<List<UserModel>> GetAllUsers();
    Task<UserModel> GetUserById(string userId);
    Task<UserModel> CreateEmployee(UserModel model);
    Task<UserModel> UpdateUser(string userId, UserModel model);
    Task<bool> DeleteUser(string userId);
}