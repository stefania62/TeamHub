using TeamHub.API.Models;

namespace TeamHub.API.Interfaces;
    public interface IAuthService
    {
        Task<string> AuthenticateUser(LoginModel model);
    }

