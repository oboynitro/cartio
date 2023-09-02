using Cartio.Application.Abstractions.Repositories;
using Cartio.Application.Abstractions.Services;
using Cartio.Application.Errors;
using Cartio.Application.Services;
using Cartio.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cartio.Tests
{
    public class AuthenticationServiceTests
    {
        private readonly AuthenticationService _sut;
        private readonly Mock<IUsersRepository> _userRepoMoq = new Mock<IUsersRepository>();
        private readonly Mock<IPasswordService> _passwordServiceMoq = new Mock<IPasswordService>();
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMoq = new Mock<IJwtTokenService>();

        public AuthenticationServiceTests()
        {
            _sut = new AuthenticationService(
                _userRepoMoq.Object,
                _jwtTokenServiceMoq.Object,
                _passwordServiceMoq.Object);
        }

        [Fact]
        public async Task AuthenticateUser_ShouldThrowException_WhenUserPhoneNumberDoesNotExist()
        {
            // Arrange
            _userRepoMoq.Setup(x => x.GetUserByPhoneNumberAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            var response = _sut.Authenticate(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            await Assert.ThrowsAsync<AuthenticationException>(() => response);
        }

        [Fact]
        public async Task AuthenticateUser_ShouldThrowException_WhenUserPasswordDoesNotMatch()
        {
            // Arrange
            _passwordServiceMoq.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            // Act
            var response = _sut.Authenticate(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            await Assert.ThrowsAsync<AuthenticationException>(() => response);
        }


        [Fact]
        public async Task AuthenticateUser_ShouldReturnFullNameAndAccessToken_WhenUserPasswordMatch()
        {
            var user = new User(
                fullName: "Frank Boakye",
                phoneNumber: "0000000000",
                password: "xxxxx",
                salt: "xxxxx"
                );

            _userRepoMoq.Setup(x => x.GetUserByPhoneNumberAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _passwordServiceMoq.Setup(x => x.VerifyPassword(It.IsAny<string>(), user.Password))
                .Returns(true);

            _jwtTokenServiceMoq.Setup(x => x.GenerateToken(user))
                .Returns("samplejwttoken");

            var response = await _sut.Authenticate(user.PhoneNumber, It.IsAny<string>());

            Assert.True(!string.IsNullOrEmpty(response.AccessToken));
            Assert.Equal(user.FullName, response.FullName);
            Assert.Equal("samplejwttoken", response.AccessToken);
        }
    }
}
