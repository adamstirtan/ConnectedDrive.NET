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
			Account account = new Account("adamstirtan@gmail.com", "o5sboVazcGLLO6&K5RpN&GVO6", Regions.NorthAmerica);

			ConnectedDrive api = new ConnectedDrive(account);

			var vehicles = await api.GetVehicles();

			Assert.IsNotNull(vehicles);
		}
	}
}

