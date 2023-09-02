using Cartio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cartio.Application.Abstractions.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserAndItemIdAsync(User user, Guid itemId);
        Task<Cart?> GetByIdAsync(Guid id);
        IQueryable<Cart?> GetAllByUserAsync(User user);
        IQueryable<Cart?> GetAll();
        Task UpdateAsync(Cart entity, int quantity);
        Task DeleteAsync(Cart entity);
        Task AddAsync(Cart entity);
    }
}
