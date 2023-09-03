using Cartio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cartio.Application.Abstractions.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetByPhoneNumberAndItemIdAsync(string phoneNumber, Guid itemId);
        Task<Cart> GetByIdAsync(Guid id);
        Task<IEnumerable<Cart>> GetAllByPhoneNumberAsync(string phoneNumber);
        Task<IEnumerable<Cart>> GetAll();
        Task UpdateAsync(Cart entity, int quantity);
        Task DeleteAsync(Cart entity);
        Task AddAsync(Cart entity);
    }
}
