namespace ConnectedDrive
{
	public class Account
	{
		public string UserName { get; set; }

		public string Password { get; set; }

		public Regions Region { get; set; }

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

