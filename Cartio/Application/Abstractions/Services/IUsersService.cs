﻿using Cartio.DTOs.Requests;
using Cartio.DTOs.Responses;
using System.Threading.Tasks;

namespace Cartio.Application.Abstractions.Services
{
    public interface IUsersService
    {
        Task<AuthenticationResult> CreateNewUser(RegisterUserRequest request);
    }
}
