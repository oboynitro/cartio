using Cartio.Application.Abstractions.Repositories;
using Cartio.Application.Errors;
using Cartio.Application.Services;
using Cartio.DTOs.Requests;
using Cartio.Entities;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Cartio.Tests.Systems.Services
{
    public class CartServiceTests
    {
        private readonly CartService _cartService;
        private readonly Mock<ICartRepository> _cartRepository = new Mock<ICartRepository>();

        public CartServiceTests()
        {
            _cartService = new CartService(_cartRepository.Object);
        }

        [Fact]
        public async Task AddItemToCart_ShouldReturnNewItem_WhenItemNotAlreadyInCart()
        {
            // Arrange
            var phoneNumber = "0000000000";
            var itemId = Guid.NewGuid();

            _cartRepository.Setup(x => x.GetByPhoneNumberAndItemIdAsync(phoneNumber, itemId))
                .ReturnsAsync(() => null);

            // Act
            var cartResponse = await _cartService.AddItemToCart(
                new AddCartItemRequest { ItemId = itemId }, phoneNumber);

            // Assert
            _cartRepository.Verify(x => x.AddAsync(It.IsAny<Cart>()));
            Assert.NotNull(cartResponse);
            Assert.Equal(1, cartResponse.Quantity);
            Assert.Equal(itemId, cartResponse.ItemId);
            Assert.Equal(phoneNumber, cartResponse.PhoneNumber);
        }

        [Fact]
        public async Task AddItemToCart_ShouldReturnNewItemWithSpecifiedQuantity_WhenItemNotAlreadyInCart()
        {
            // Arrange
            var phoneNumber = "0000000000";
            var cartRequest = new AddCartItemRequest { ItemId = Guid.NewGuid(), Quantity = 5 };

            _cartRepository.Setup(x => x.GetByPhoneNumberAndItemIdAsync(
                phoneNumber, cartRequest.ItemId))
                .ReturnsAsync(() => null);

            // Act
            var cartResponse = await _cartService.AddItemToCart(cartRequest, phoneNumber);

            // Assert
            _cartRepository.Verify(x => x.AddAsync(It.IsAny<Cart>()));
            Assert.NotNull(cartResponse);
            Assert.Equal(cartRequest.Quantity, cartResponse.Quantity);
            Assert.Equal(cartRequest.ItemId, cartResponse.ItemId);
            Assert.Equal(phoneNumber, cartResponse.PhoneNumber);
        }

        [Fact]
        public async Task AddItemToCart_ShouldUpdateItemQuantity_WhenItemAlreadyInCart()
        {
            // Arrange
            var cartRequest = new AddCartItemRequest { ItemId = Guid.NewGuid() };
            var phoneNumber = "0000000000";

            var cartItem = new Cart {
                Id = Guid.NewGuid(),
                ItemId = cartRequest.ItemId,
                ItemName = "Item 1",
                Quantity = 1,
                UnitPrice = 250,
                PhoneNumber = phoneNumber 
            };

            _cartRepository.Setup(x => x.GetByPhoneNumberAndItemIdAsync(
                phoneNumber, cartRequest.ItemId))
                .ReturnsAsync(cartItem);

            // Act
            var cartResponse = await _cartService.AddItemToCart(cartRequest, phoneNumber);

            // Assert
            _cartRepository.Verify(x => x.UpdateAsync(It.IsAny<Cart>(), It.IsAny<int>()));
            Assert.NotNull(cartResponse);
            Assert.Equal(cartRequest.ItemId, cartResponse.ItemId);
        }

        [Fact]
        public async Task GetCartItemById_ShouldThrowException_WhenNoItemIsFound()
        {
            // Arrange
            var passedId = Guid.NewGuid();

            _cartRepository.Setup(x => x.GetByIdAsync(passedId))
                .ReturnsAsync(() => null);

            // Act
            var cartResponse = _cartService.GetCartItemById(passedId);

            // Assert
            await Assert.ThrowsAsync<CartNotFoundException>(() => cartResponse);
        }

        [Fact]
        public async Task GetCartItemById_ShouldReturnCartItem_WhenCartIdIsPassed()
        {
            // Arrange
            var cartItem = new Cart
            {
                Id = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                Quantity = 1,
                UnitPrice = 250,
                PhoneNumber = "0000000000"
            };

            _cartRepository.Setup(x => x.GetByIdAsync(cartItem.Id))
                .ReturnsAsync(cartItem);

            // Act
            var cartResponse = await _cartService.GetCartItemById(cartItem.Id);

            //Assert
            Assert.NotNull(cartResponse);
            Assert.Equal(cartItem.Id, cartResponse.Id);
            Assert.Equal(cartItem.Quantity, cartResponse.Quantity);
            Assert.Equal(cartItem.ItemName, cartResponse.ItemName);
        }

        [Fact]
        public async Task RemoveItemFromCart_ShouldThrowException_WhenUserCartNotFound()
        {
            // Arrange
            _cartRepository.Setup(x => x.GetByPhoneNumberAndItemIdAsync(
                It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var cartResponse = _cartService.RemoveItemFromCart(
                new RemoveCartItemRequest(), It.IsAny<string>());

            // Assert
            await Assert.ThrowsAsync<CartNotFoundException>(() => cartResponse);
        }

        [Fact]
        public async Task RemoveItemFromCart_ShouldUpdateItemQuantity_WhenClearIsFalse()
        {
            // Arrange
            var cartItem = new Cart
            {
                Id = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                ItemName = "Item 1",
                Quantity = 5,
                UnitPrice = 250,
                PhoneNumber = "0000000000"
            };

            var requestItem = new RemoveCartItemRequest { ItemId = cartItem.ItemId };

            _cartRepository.Setup(x => x.GetByPhoneNumberAndItemIdAsync(
                cartItem.PhoneNumber, cartItem.ItemId))
                .ReturnsAsync(cartItem);

            // Act
            var cartResponse = await _cartService.RemoveItemFromCart(
                requestItem, cartItem.PhoneNumber);

            // Assert
            _cartRepository.Verify(x => x.UpdateAsync(It.IsAny<Cart>(), It.IsAny<int>()));
            Assert.NotNull(cartResponse);
            Assert.Equal(cartItem.ItemId, cartResponse.ItemId);
        }

        [Fact]
        public async Task RemoveItemFromCart_ShouldDeleteCartItem_WhenQuantityIsLessThanOne()
        {
            // Arrange
            var cartItem = new Cart
            {
                Id = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                ItemName = "Item 1",
                Quantity = 0,
                UnitPrice = 250,
                PhoneNumber = "0000000000"
            };

            var requestItem = new RemoveCartItemRequest { ItemId = cartItem.ItemId };

            _cartRepository.Setup(x => x.GetByPhoneNumberAndItemIdAsync(
                cartItem.PhoneNumber, cartItem.ItemId))
                .ReturnsAsync(cartItem);

            // Act
            var cartResponse = await _cartService.RemoveItemFromCart(
                requestItem, cartItem.PhoneNumber);

            // Assert
            _cartRepository.Verify(x => x.UpdateAsync(It.IsAny<Cart>(), It.IsAny<int>()));
            _cartRepository.Verify(x => x.DeleteAsync(It.IsAny<Cart>()));
            Assert.Null(cartResponse);
        }

        [Fact]
        public async Task RemoveItemFromCart_ShouldDeleteCartItem_WhenClearIsTrue()
        {
            // Arrange
            var cartItem = new Cart
            {
                Id = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                ItemName = "Item 1",
                Quantity = 5,
                UnitPrice = 250,
                PhoneNumber = "0000000000"
            };

            var requestItem = new RemoveCartItemRequest { 
                ItemId = cartItem.ItemId, ClearItem = true 
            };

            _cartRepository.Setup(x => x.GetByPhoneNumberAndItemIdAsync(
                cartItem.PhoneNumber, cartItem.ItemId))
                .ReturnsAsync(cartItem);

            // Act
            var cartResponse = await _cartService.RemoveItemFromCart(
                requestItem, cartItem.PhoneNumber);

            // Assert
            _cartRepository.Verify(x => x.DeleteAsync(It.IsAny<Cart>()));
            Assert.Null(cartResponse);
        }
    }
}
