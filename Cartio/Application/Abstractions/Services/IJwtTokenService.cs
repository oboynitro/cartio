using Cartio.Entities;

namespace Cartio.Application.Abstractions.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
