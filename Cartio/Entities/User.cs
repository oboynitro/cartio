using Cartio.Entities.Common;

namespace Cartio.Entities
{
    public sealed class User : BaseEntity
    {
        public User()
        {}

        public User(
            string fullName,
            string phoneNumber,
            string password,
            string salt)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Password = password;
            Salt = salt;
        }

        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Salt { get; set; } = null!;
    }
}
