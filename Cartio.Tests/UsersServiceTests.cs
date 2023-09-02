using Cartio.Application.Abstractions.Repositories;
using Cartio.Application.Abstractions.Services;
using Cartio.Application.Errors;
using Cartio.Application.Services;
using Cartio.DTOs.Requests;
using Cartio.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cartio.Tests
{
    public class UsersServiceTests
    {
        private readonly UserService _sut;
        private readonly Mock<IUsersRepository> _userRepoMoq = new Mock<IUsersRepository>();
        private readonly Mock<IPasswordService> _passwordServiceMoq = new Mock<IPasswordService>();
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMoq = new Mock<IJwtTokenService>();

        public UsersServiceTests()
        {
            _sut = new UserService(
                _userRepoMoq.Object,
                _passwordServiceMoq.Object,
                _jwtTokenServiceMoq.Object);
        }

        [Fact]
        public async Task CreateNewUser_ShouldThrowException_WhenUserPhoneNumberAlreadyExists()
        {
            // Arrange
            var requestData = new RegisterUserRequest
            {
                FullName = "Frank",
                PhoneNumber = "0572868097",
                Password = "password",
                ConfirmPassword = "password",
            };

            _userRepoMoq.Setup(x => x.GetUserByPhoneNumberAsync(requestData.PhoneNumber))
                .ReturnsAsync(new User());

            // Act
            var response = _sut.CreateNewUser(requestData);

            // Assert
            await Assert.ThrowsAsync<DuplicatePhoneNumberException>(() => response);
        }

        [Fact]
        public async Task CreateNewUser_ShouldReturnCreatedUser_WhenUserPhoneNumberDoesNotExist()
        {
            // Arrange
            var requestData = new RegisterUserRequest
            {
                FullName = "Frank",
                PhoneNumber = "0572868097",
                Password = "password",
                ConfirmPassword = "password",
            };
            

            _userRepoMoq.Setup(x => x.GetUserByPhoneNumberAsync(requestData.PhoneNumber))
                .ReturnsAsync(() => null);

            // Act
            var newUser = await _sut.CreateNewUser(requestData);

            // Assert
            Assert.Equal(newUser.FullName, requestData.FullName);
        }
    }
}
