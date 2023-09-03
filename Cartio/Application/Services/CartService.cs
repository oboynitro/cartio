using Cartio.Application.Abstractions.Repositories;
using Cartio.Application.Abstractions.Services;
using Cartio.Application.Errors;
using Cartio.Application.Utils;
using Cartio.DTOs.Requests;
using Cartio.DTOs.Responses;
using Cartio.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cartio.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository ;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CartItemResponse> AddItemToCart(
            AddCartItemRequest request, string phoneNumber)
        {
            var cartItem = await _cartRepository.GetByPhoneNumberAndItemIdAsync(
                phoneNumber, request.ItemId);

            if (cartItem == null)
            {
                var newCartItem = new Cart(
                    itemId: request.ItemId,
                    itemName: request.ItemName,
                    quantity: request.Quantity > 0 ? request.Quantity : 1,
                    unitPrice: request.UnitPrice,
                    phoneNumber: phoneNumber);

                await _cartRepository.AddAsync(newCartItem);

                return new CartItemResponse 
                { 
                    Id = newCartItem.Id,
                    ItemId = newCartItem.ItemId,
                    ItemName = newCartItem.ItemName,
                    Quantity = newCartItem.Quantity,
                    UnitPrice = newCartItem.UnitPrice,
                    PhoneNumber = newCartItem.PhoneNumber
                };
            }

            await _cartRepository.UpdateAsync(cartItem, cartItem.Quantity + 1);

            return new CartItemResponse
            {
                Id = cartItem.Id,
                ItemId = cartItem.ItemId,
                ItemName = cartItem.ItemName,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice,
                PhoneNumber = cartItem.PhoneNumber
            };
        }

        public async Task<Paginator<CartItemResponse>> AllCartItems(CartsFilterQueryRequest query)
        {
            var cartsItemsQuery = await _cartRepository.GetAll();

            if(!string.IsNullOrWhiteSpace(query.PhoneNumber))
                cartsItemsQuery = cartsItemsQuery.Where(c => c.PhoneNumber.Contains(query.PhoneNumber));

            if (!string.IsNullOrWhiteSpace(query.ItemName))
                cartsItemsQuery = cartsItemsQuery.Where(c => c.ItemName.Contains(query.ItemName));

            if (query.Quantity > 0)
                cartsItemsQuery = cartsItemsQuery.Where(c => c.Quantity == query.Quantity);

            if (!string.IsNullOrWhiteSpace(query.Time))
            {
                DateTime.TryParse(query.Time, out DateTime time);
                cartsItemsQuery = cartsItemsQuery.Where(
                    c => DateTime.Compare(c.CreatedAt, time) == 0);
            }


            var itemResponsesQuery = cartsItemsQuery.Select(
                c => new CartItemResponse
                {
                    Id = c.Id,
                    ItemId = c.ItemId,
                    ItemName = c.ItemName,
                    UnitPrice = c.UnitPrice,
                    Quantity = c.Quantity,
                    PhoneNumber = c.PhoneNumber
                });

            var cartItems = Paginator<CartItemResponse>.CreateAsync(
                itemResponsesQuery, query.Page, query.ItemsPerPage);

            return cartItems;
        }

        public async Task<Paginator<CartItemResponse>> AllUserCartItems(
            int page,
            int itemsPerPage,
            string phoneNumber)
        {
            var cartItemsQuery = await _cartRepository.GetAllByPhoneNumberAsync(phoneNumber);

            var cartItemsResponses = cartItemsQuery
                .Select(c => new CartItemResponse
                {
                    Id = c.Id,
                    ItemId = c.ItemId,
                    ItemName = c.ItemName,
                    UnitPrice = c.UnitPrice,
                    Quantity = c.Quantity,
                    PhoneNumber = c.PhoneNumber
                });

            var cartItems = Paginator<CartItemResponse>
                .CreateAsync(cartItemsResponses, page, itemsPerPage);

            return cartItems;
        }


        public async Task<CartItemResponse> GetCartItemById(Guid id)
        {
            var cartItem = await _cartRepository.GetByIdAsync(id)
                ?? throw new CartNotFoundException();

            return new CartItemResponse
            {
                Id = cartItem.Id,
                ItemId = cartItem.ItemId,
                ItemName = cartItem.ItemName,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice,
            };
        }

        public async Task<CartItemResponse?> RemoveItemFromCart(RemoveCartItemRequest request, string phoneNumber)
        {
            var cartItem = await _cartRepository.GetByPhoneNumberAndItemIdAsync(
                phoneNumber, request.ItemId)
                ?? throw new CartNotFoundException();

            if(request.ClearItem)
            {
                await _cartRepository.DeleteAsync(cartItem);
                return null;
            }

            await _cartRepository.UpdateAsync(cartItem, cartItem.Quantity - 1);

            if (cartItem.Quantity < 1)
            {
                await _cartRepository.DeleteAsync(cartItem);
                return null;
            }

            return new CartItemResponse
            {
                Id = cartItem.Id,
                ItemId = cartItem.ItemId,
                ItemName = cartItem.ItemName,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice,
            };
        }
    }
}
