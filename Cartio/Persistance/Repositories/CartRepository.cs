using Cartio.Application.Abstractions.Repositories;
using Cartio.Entities;
using Cartio.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cartio.Api.Persistance.Repositories
{
    internal class CartRepository : ICartRepository
    {
        private readonly AppDataContext _context;

        public CartRepository(AppDataContext context) => _context = context;

        public async Task AddAsync(Cart entity)
        {
            await _context.Carts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Cart entity)
        {
            _context.Carts.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetByIdAsync(Guid id)
        {
            return await _context.Carts
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cart> GetByPhoneNumberAndItemIdAsync(string phoneNumber, Guid id)
        {
            return await _context.Carts
                .Where(c => c.PhoneNumber == phoneNumber && c.ItemId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Cart>> GetAllByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Carts
                .Where(c => c.PhoneNumber == phoneNumber)
                .ToListAsync();
        }

        public async Task UpdateAsync(Cart entity, int quantity)
        {
            entity.Quantity = quantity;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cart>> GetAll()
        {
            return await _context.Carts.ToListAsync();
        }
    }
}
