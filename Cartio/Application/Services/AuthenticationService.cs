using Cartio.Application.Abstractions.Repositories;
using Cartio.Application.Abstractions.Services;
using Cartio.Application.Errors;
using Cartio.DTOs.Responses;
using System.Threading.Tasks;

namespace Cartio.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordService _passwordService;

        public AuthenticationService(
            IUsersRepository usersRepository,
            IJwtTokenService jwtTokenService,
            IPasswordService passwordService)
        {
            _usersRepository = usersRepository;
            _jwtTokenService = jwtTokenService;
            _passwordService = passwordService;
        }

        public async Task<AuthenticationResult> Authenticate(string phoneNumber, string password)
        {
            var user = await _usersRepository.GetUserByPhoneNumberAsync(phoneNumber)
                ?? throw new AuthenticationException();

            if (!_passwordService.VerifyPassword(string.Concat(password, user.Salt), user.Password))
                throw new AuthenticationException();

            var token = _jwtTokenService.GenerateToken(user);

            return new AuthenticationResult
            {
                FullName = user.FullName,
                AccessToken = token
            };

        }
    }
}
