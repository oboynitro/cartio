namespace Cartio.Application.Abstractions.Services
{
    public interface IPasswordService
    {
        string ComputeSalt(int size = 64);
        (string hash, string salt) HashPassword(string rawPassword);
        bool VerifyPassword(string rawPassword, string hashedPassword);
    }
}
