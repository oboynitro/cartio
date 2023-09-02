using Cartio.Application.Abstractions.Services;
using Cartio.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cartio.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _usersService;
        private readonly IAuthenticationService _authenticationService;

        public AuthController(
            IUserService usersService, 
            IAuthenticationService authenticationService)
        {
            _usersService = usersService;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// Fullname: Full registered name of user
        /// Token: JWT access token to use
        /// </returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /auth/register
        ///     {
        ///        "phoneNumber": "0000000000",
        ///        "fullName": "CartioUser",
        ///        "password": "password",
        ///        "confirmPassword": "password",
        ///     }
        ///
        /// </remarks>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var authResult = await _usersService.CreateNewUser(request);

            return CreatedAtAction("Register", authResult);
        }


        /// <summary>
        /// Authenticats user and responses with a jwt token
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// Fullname: Full registered name of user
        /// Token: JWT access token to use
        /// </returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /auth/login
        ///     {
        ///        "phoneNumber": "0000000000",
        ///        "password": "password",
        ///     }
        ///
        /// </remarks>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var authResult = await _authenticationService.Authenticate(
                request.PhoneNumber, request.Password);

            return Ok(authResult);
        }
    }
}
