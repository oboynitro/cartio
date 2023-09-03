using System.Linq;
using System;
using System.Threading.Tasks;
using Cartio.Entities;
using System.Collections.Generic;

namespace Cartio.Application.Abstractions.Repositories
{
    public interface IUsersRepository
    {
        Task AddAsync(User entity);

        Task<User?> GetUserByPhoneNumberAsync(string phoneNumber);
    }
}
