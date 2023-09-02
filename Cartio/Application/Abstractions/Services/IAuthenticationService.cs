using Cartio.DTOs.Responses;
using System.Threading.Tasks;

namespace Cartio.Application.Abstractions.Services
{
    public interface IAuthenticationService
    {
        public Task<AuthenticationResult> Authenticate(string phoneNumber, string password);
    }
}
