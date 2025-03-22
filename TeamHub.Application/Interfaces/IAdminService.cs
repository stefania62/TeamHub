using TeamHub.Application.Models;
using TeamHub.Application.Result;

namespace TeamHub.Application.Interfaces;
public interface IAdminService
{
    Task<Result<List<UserModel>>> GetAllUsers();
    Task<Result<UserModel>> GetUserById(string userId);
    Task<Result<UserModel>> CreateEmployee(UserModel model);
    Task<Result<UserModel>> UpdateUser(string userId, UserProfile model);
    Task<Result<bool>> DeleteUser(string userId);
}