using System;

using ConnectedDrive.Models;

namespace ConnectedDrive.Tests
{
	[TestClass]
	public class GetVehiclesTest
	{
		[TestMethod]
		public async Task GetVehiclesIsNotNull()
		{
			Account account = new Account("user@gmail.com", "password", Regions.NorthAmerica);

			ConnectedDrive api = new ConnectedDrive(account);

			var vehicles = await api.GetVehicles();

			Assert.IsNotNull(vehicles);
		}
	}
}

