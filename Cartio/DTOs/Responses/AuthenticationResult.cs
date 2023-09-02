namespace Cartio.DTOs.Responses
{
    public class AuthenticationResult
    {
        public string FullName { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
    }
}
