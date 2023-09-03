using Cartio.Application.Abstractions.Repositories;
using Cartio.Api.Persistance;
using Cartio.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Cartio.Persistance.Repositories
{
    public sealed class UsersRepository : IUsersRepository
    {
        private readonly AppDataContext _context;

        public UsersRepository(AppDataContext context) => _context = context;

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users
                .Where(u => u.PhoneNumber == phoneNumber)
                .FirstOrDefaultAsync();
        }
    }
}
