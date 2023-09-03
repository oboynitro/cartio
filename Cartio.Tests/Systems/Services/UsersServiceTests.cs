using Cartio.Application.Abstractions.Repositories;
using Cartio.Application.Abstractions.Services;
using Cartio.Application.Errors;
using Cartio.Application.Services;
using Cartio.DTOs.Requests;
using Cartio.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cartio.Tests.Systems.Services
{
    public class UsersServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IUsersRepository> _userRepoMoq = new Mock<IUsersRepository>();
        private readonly Mock<IPasswordService> _passwordServiceMoq = new Mock<IPasswordService>();
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMoq = new Mock<IJwtTokenService>();

        public UsersServiceTests()
        {
            _userService = new UserService(
                _userRepoMoq.Object,
                _passwordServiceMoq.Object,
                _jwtTokenServiceMoq.Object);
        }

        [Fact]
        public async Task CreateNewUser_ShouldThrowException_WhenUserPhoneNumberIsNotNumeric()
        {
            // Arrange
            var phoneNumber = "somethingbad";

            // Act
            var response = _userService.CreateNewUser(
                new RegisterUserRequest
                {
                    FullName = It.IsAny<string>(),
                    PhoneNumber = phoneNumber,
                    Password = It.IsAny<string>(),
                    ConfirmPassword = It.IsAny<string>()
                });

            // Assert
            await Assert.ThrowsAsync<InvalidPhoneNumberException>(() => response);
        }

        [Fact]
        public async Task CreateNewUser_ShouldThrowException_WhenUserPhoneNumberIsNumericButLessThan10()
        {
            // Arrange
            var phoneNumber = "00000000";

            // Act
            var response = _userService.CreateNewUser(
                new RegisterUserRequest
                {
                    FullName = It.IsAny<string>(),
                    PhoneNumber = phoneNumber,
                    Password = It.IsAny<string>(),
                    ConfirmPassword = It.IsAny<string>()
                });

            // Assert
            await Assert.ThrowsAsync<InvalidPhoneNumberException>(() => response);
        }

        [Fact]
        public async Task CreateNewUser_ShouldThrowException_WhenUserPhoneNumberIsNumericButGreaterThan10()
        {
            // Arrange
            var phoneNumber = "000000000000";

            // Act
            var response = _userService.CreateNewUser(
                new RegisterUserRequest
                {
                    FullName = It.IsAny<string>(),
                    PhoneNumber = phoneNumber,
                    Password = It.IsAny<string>(),
                    ConfirmPassword = It.IsAny<string>()
                });

            // Assert
            await Assert.ThrowsAsync<InvalidPhoneNumberException>(() => response);
        }

        [Fact]
        public async Task CreateNewUser_ShouldReturnCreatedUser_WhenUserPhoneNumberIsNumericAndEqualTo10()
        {
            // Arrange
            var phoneNumber = "0000000000";

            _userRepoMoq.Setup(x => x.GetUserByPhoneNumberAsync(phoneNumber))
                .ReturnsAsync(() => null);

            _passwordServiceMoq.Setup(x => x.HashPassword(It.IsAny<string>()))
                .Returns((It.IsAny<string>(), It.IsAny<string>()));

            _jwtTokenServiceMoq.Setup(x => x.GenerateToken(It.IsAny<User>()))
                .Returns("access-token");

            // Act
            var response = await _userService.CreateNewUser(
                new RegisterUserRequest
                {
                    PhoneNumber = phoneNumber,
                    FullName = "Frank",
                    Password = "password",
                    ConfirmPassword = "password"
                });

            // Assert
            _userRepoMoq.Verify(x => x.GetUserByPhoneNumberAsync(phoneNumber));
            _userRepoMoq.Verify(x => x.AddAsync(It.IsAny<User>()));
            _passwordServiceMoq.Verify(x => x.HashPassword(It.IsAny<string>()));
            _jwtTokenServiceMoq.Verify(x => x.GenerateToken(It.IsAny<User>()));
            Assert.NotNull(response);
            Assert.Equal("Frank", response.FullName);
            Assert.Equal("access-token", response.AccessToken);
        }


        [Fact]
        public async Task CreateNewUser_ShouldThrowException_WhenUserPhoneNumberAlreadyExists()
        {
            // Arrange
            _userRepoMoq.Setup(x => x.GetUserByPhoneNumberAsync("0000000000"))
                .ReturnsAsync(new User());

            // Act
            var response = _userService.CreateNewUser(
                new RegisterUserRequest { 
                    FullName = It.IsAny<string>(), 
                    PhoneNumber = "0000000000", 
                    Password = It.IsAny<string>(), 
                    ConfirmPassword = It.IsAny<string>()});

            // Assert
            _userRepoMoq.Verify(x => x.GetUserByPhoneNumberAsync("0000000000"));
            await Assert.ThrowsAsync<DuplicatePhoneNumberException>(() => response);
        }


        [Fact]
        public async Task CreateNewUser_ShouldReturnCreatedUser_WhenUserPhoneNumberDoesNotExist()
        {
            // Arrange
            _userRepoMoq.Setup(x => x.GetUserByPhoneNumberAsync("0000000000"))
                .ReturnsAsync(() => null);

            _passwordServiceMoq.Setup(x => x.HashPassword(It.IsAny<string>()))
                .Returns((It.IsAny<string>(), It.IsAny<string>()));

            _jwtTokenServiceMoq.Setup(x => x.GenerateToken(It.IsAny<User>()))
                .Returns("access-token");

            // Act
            var response = await _userService.CreateNewUser(
                new RegisterUserRequest
                {
                    PhoneNumber = "0000000000",
                    FullName = "Frank",
                    Password = "password",
                    ConfirmPassword = "password"
                });

            // Assert
            _userRepoMoq.Verify(x => x.GetUserByPhoneNumberAsync("0000000000"));
            _userRepoMoq.Verify(x => x.AddAsync(It.IsAny<User>()));
            _passwordServiceMoq.Verify(x => x.HashPassword(It.IsAny<string>()));
            _jwtTokenServiceMoq.Verify(x => x.GenerateToken(It.IsAny<User>()));
            Assert.NotNull(response);
            Assert.Equal("Frank", response.FullName);
            Assert.Equal("access-token", response.AccessToken);
        }
    }
}
