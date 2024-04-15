using ConnectedDrive.Models;

namespace ConnectedDrive
{
    public class Account(
            string userName,
            string password,
            Regions region)
    {
        public string UserName { get; private set; } = userName;

        public string Password { get; set; } = password;

        public Regions Region { get; set; } = region;

        public string? AccessToken { get; set; }
    }
}