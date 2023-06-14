using ConnectedDrive.Models;

namespace ConnectedDrive
{
	public class Account
	{
		public string UserName { get; private set; }

		public string Password { get; set; }

		public Regions Region { get; set; }

		public string? AccessToken { get; set; }

		public Account(
			string userName,
			string password,
			Regions region)
		{
			UserName = userName;
			Password = password;
			Region = region;
		}


	}
}
