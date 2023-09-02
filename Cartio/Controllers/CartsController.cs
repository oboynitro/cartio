using Cartio.Application.Abstractions.Services;
using Cartio.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cartio.Controllers
{
    [ApiController]
    [Authorize]
    [Route("carts")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart(AddCartItemRequest request)
        {
            var cartResponse = await _cartService.AddItemToCart(
                request, GetAuthUserPhoneNumber());

            return CreatedAtAction("AddItemToCart", cartResponse);
        }


        [HttpGet("me")]
        public async Task<IActionResult> ViewUserCartItems(int page = 1, int itemsPerPage = 20)
        {
            var cartResponse = await _cartService.AllUserCartItems(
                page,
                itemsPerPage,
                GetAuthUserPhoneNumber());

            return Ok(cartResponse);
        }


        [HttpGet]
        public async Task<IActionResult> ViewCartItems([FromQuery]CartsFilterQueryRequest query)
        {
            var cartResponse = await _cartService.AllCartItems(query);

            return Ok(cartResponse);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ViewSingleCartItem(Guid id)
        {
            var cartResponse = await _cartService.GetCartItemById(id);

            return Ok(cartResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveItemFromCart(RemoveCartItemRequest request)
        {
            await _cartService.RemoveItemFromCart(request, GetAuthUserPhoneNumber());

            return NoContent();
        }


        private string GetAuthUserPhoneNumber()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

    }
}
