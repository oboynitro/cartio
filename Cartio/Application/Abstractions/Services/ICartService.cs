using Cartio.Application.Utils;
using Cartio.DTOs.Requests;
using Cartio.DTOs.Responses;
using System;
using System.Threading.Tasks;

namespace Cartio.Application.Abstractions.Services
{
    public interface ICartService
    {
        public Task<CartItemResponse> AddItemToCart(
            AddCartItemRequest request, 
            string phoneNumber);
        public Task<Paginator<CartItemResponse>> AllUserCartItems(
            int page, 
            int itemsPerPage, 
            string phoneNumber);
        public Task<Paginator<CartItemResponse>> AllCartItems(CartsFilterQueryRequest query);
        public Task RemoveItemFromCart(RemoveCartItemRequest request, string phoneNumber);
        public Task<CartItemResponse> GetCartItemById(Guid id);
    }
}
