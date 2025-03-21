using TeamHub.Application.Models;

namespace TeamHub.Application.Interfaces;
    public interface IAuthService
    {
        Task<string> AuthenticateUser(LoginModel model);
    }

