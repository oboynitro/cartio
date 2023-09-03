using Cartio.Application.Abstractions.Services;
using Cartio.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Adds item to cart
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /carts
        ///     {
        ///        "itemId": "0f6ef74a-5ccb-4863-a3a8-c0322406cd95",
        ///        "itemName": "Test item",
        ///        "quantity": 0,
        ///        "unitPrice": 250
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddItemToCart(AddCartItemRequest request)
        {
            var cartResponse = await _cartService.AddItemToCart(
                request, GetAuthUserPhoneNumber());

            return CreatedAtAction("AddItemToCart", cartResponse);
        }

        /// <summary>
        /// Get authenticated user's cart
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ViewUserCartItems(int page = 1, int itemsPerPage = 20)
        {
            var cartResponse = await _cartService.AllUserCartItems(
                page,
                itemsPerPage,
                GetAuthUserPhoneNumber());

            return Ok(cartResponse);
        }

        /// <summary>
        /// Get all cart
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ViewCartItems([FromQuery]CartsFilterQueryRequest query)
        {
            var cartResponse = await _cartService.AllCartItems(query);

            return Ok(cartResponse);
        }

        /// <summary>
        /// Get single cart item when id is provided
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ViewSingleCartItem(Guid id)
        {
            var cartResponse = await _cartService.GetCartItemById(id);

            return Ok(cartResponse);
        }

        /// <summary>
        /// Removes item from cart when item id is provided
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveItemFromCart(RemoveCartItemRequest request)
        {
            var cartResponse = await _cartService.RemoveItemFromCart(request, GetAuthUserPhoneNumber());

            if (cartResponse == null) return NoContent();

            return Ok(cartResponse);
        }


        private string GetAuthUserPhoneNumber()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

    }
}
