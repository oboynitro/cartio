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

        public async Task<Cart?> GetByIdAsync(Guid id)
        {
            return await _context.Carts
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cart?> GetByUserAndItemIdAsync(User user, Guid id)
        {
            return await _context.Carts
                .Where(c => c.User == user && c.ItemId == id)
                .Include(c => c.User)
                .FirstOrDefaultAsync();
        }

        public IQueryable<Cart?> GetAllByUserAsync(User user)
        {
            return _context.Carts
                .Where(c => c.User == user)
                .Include(c => c.User)
                .AsQueryable();
        }

        public async Task UpdateAsync(Cart entity, int quantity)
        {
            entity.Quantity = quantity;
            await _context.SaveChangesAsync();
        }

        public IQueryable<Cart?> GetAll()
        {
            return _context.Carts
                .Include(c => c.User)
                .AsQueryable();
        }
    }
}
