﻿using Microsoft.AspNetCore.Http;
using TeamHub.API.Models;

namespace TeamHub.Application.Interfaces;
public interface IUserService
{
    Task<UserModel> GetProfile(string userId);
    Task<bool> UpdateProfile(string userId, UserModel model);
}
