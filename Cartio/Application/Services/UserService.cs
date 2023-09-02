using Cartio.Application.Abstractions.Repositories;
using Cartio.Application.Abstractions.Services;
using Cartio.Application.Errors;
using Cartio.DTOs.Requests;
using Cartio.DTOs.Responses;
using Cartio.Entities;
using System.Threading.Tasks;

namespace Cartio.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtTokenService _jwtTokenGenerator;

        public UserService(
            IUsersRepository usersRepository,
            IPasswordService passwordService,
            IJwtTokenService jwtTokenGenerator)
        {
            _usersRepository = usersRepository;
            _passwordService = passwordService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthenticationResult> CreateNewUser(RegisterUserRequest request)
        {
            if (await _usersRepository.GetUserByPhoneNumberAsync(request.PhoneNumber) != null)
                throw new DuplicatePhoneNumberException();

            (string passwordHash, string salt) = _passwordService.HashPassword(request.Password);

            User newUser = new User(
                fullName: request.FullName,
                phoneNumber: request.PhoneNumber,
                password: passwordHash,
                salt: salt
                );

            await _usersRepository.AddAsync(newUser);
            string token = _jwtTokenGenerator.GenerateToken(newUser);

            return new AuthenticationResult
            {
                FullName = newUser.FullName,
                AccessToken = token
            };
        }

    }
}
