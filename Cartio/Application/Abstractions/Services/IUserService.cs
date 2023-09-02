using Cartio.DTOs.Requests;
using Cartio.DTOs.Responses;
using System.Threading.Tasks;

namespace Cartio.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<AuthenticationResult> CreateNewUser(RegisterUserRequest request);
    }
}
