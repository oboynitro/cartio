using Cartio.Application.Abstractions.Repositories;
using Cartio.Application.Abstractions.Services;
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
    public class AuthenticationServiceTests
    {
        private readonly AuthenticationService _authenticationService;
        private readonly Mock<IUsersRepository> _userRepoMoq = new Mock<IUsersRepository>();
        private readonly Mock<IPasswordService> _passwordServiceMoq = new Mock<IPasswordService>();
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMoq = new Mock<IJwtTokenService>();

        public AuthenticationServiceTests()
        {
            _authenticationService = new AuthenticationService(
                _userRepoMoq.Object,
                _jwtTokenServiceMoq.Object,
                _passwordServiceMoq.Object);
        }

        [Fact]
        public async Task Authenticate_ShouldThrowException_WhenUserPhoneNumberIsNotNumeric()
        {
            // Arrange
            var phoneNumber = "somethingbad";

            // Act
            var response = _authenticationService.Authenticate(phoneNumber, It.IsAny<string>());

            // Assert
            await Assert.ThrowsAsync<InvalidPhoneNumberException>(() => response);
        }

        [Fact]
        public async Task Authenticate_ShouldThrowException_WhenUserPhoneNumberIsNumericButLessThan10()
        {
            // Arrange
            var phoneNumber = "00000000";

            // Act
            var response = _authenticationService.Authenticate(phoneNumber, It.IsAny<string>());

            // Assert
            await Assert.ThrowsAsync<InvalidPhoneNumberException>(() => response);
        }

        [Fact]
        public async Task Authenticate_ShouldThrowException_WhenUserPhoneNumberIsNumericButGreaterThan10()
        {
            // Arrange
            var phoneNumber = "000000000000";

            // Act
            var response = _authenticationService.Authenticate(phoneNumber, It.IsAny<string>());

            // Assert
            await Assert.ThrowsAsync<InvalidPhoneNumberException>(() => response);
        }

        [Fact]
        public async Task Authenticate_ShouldThrowException_WhenUserPhoneNumberDoesNotExist()
        {
            // Arrange
            var phoneNumber = "0000000000";
            _userRepoMoq.Setup(x => x.GetUserByPhoneNumberAsync(phoneNumber))
                .ReturnsAsync(() => null);

            // Act
            var response = _authenticationService.Authenticate(phoneNumber, It.IsAny<string>());

            // Assert
            await Assert.ThrowsAsync<AuthenticationException>(() => response);
        }

        [Fact]
        public async Task Authenticate_ShouldThrowException_WhenUserPasswordDoesNotMatch()
        {
            // Arrange
            var phoneNumber = "0000000000";
            _passwordServiceMoq.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            // Act
            var response = _authenticationService.Authenticate(phoneNumber, It.IsAny<string>());

            // Assert
            await Assert.ThrowsAsync<AuthenticationException>(() => response);
        }


        [Fact]
        public async Task Authenticate_ShouldReturnUserFullName_WhenUserPhoneNumberIsNumericAndEqualTo10()
        {
            // Arrange
            var phoneNumber = "0000000000";
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Frank",
                PhoneNumber = phoneNumber,
                Password = "passhash",
                Salt = "saltxx"
            };

            _userRepoMoq.Setup(x => x.GetUserByPhoneNumberAsync(phoneNumber))
                .ReturnsAsync(user);

            _passwordServiceMoq.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);


            _jwtTokenServiceMoq.Setup(x => x.GenerateToken(It.IsAny<User>()))
                .Returns("access-token");

            // Act
            var response = await _authenticationService.Authenticate(
                phoneNumber, "password");

            // Assert
            _passwordServiceMoq.Verify(x => x.VerifyPassword(
                It.IsAny<string>(), It.IsAny<string>()));
            _jwtTokenServiceMoq.Verify(x => x.GenerateToken(It.IsAny<User>()));
            Assert.NotNull(response);
            Assert.Equal(user.FullName, response.FullName);
            Assert.Equal("access-token", response.AccessToken);
        }


        [Fact]
        public async Task Authenticate_ShouldReturnUserFullName_WhenUserPasswordMatch()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Frank",
                PhoneNumber = "0000000000",
                Password = "passhash",
                Salt = "saltxx"
            };

            _userRepoMoq.Setup(x => x.GetUserByPhoneNumberAsync(user.PhoneNumber))
                .ReturnsAsync(user);

            _passwordServiceMoq.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);


            _jwtTokenServiceMoq.Setup(x => x.GenerateToken(It.IsAny<User>()))
                .Returns("access-token");

            // Act
            var response = await _authenticationService.Authenticate(
                user.PhoneNumber, "password");

            // Assert
            _passwordServiceMoq.Verify(x => x.VerifyPassword(
                It.IsAny<string>(), It.IsAny<string>()));
            _jwtTokenServiceMoq.Verify(x => x.GenerateToken(It.IsAny<User>()));
            Assert.NotNull(response);
            Assert.Equal(user.FullName, response.FullName);
            Assert.Equal("access-token", response.AccessToken);
        }
    }
}
