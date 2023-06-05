using ConnectedDrive.Models;

namespace ConnectedDrive
{
	public class ConnectedDrive
	{
		private readonly Account _account;

		public ConnectedDrive(Account account)
		{
			_account = account;
		}

		public Task<Vehicle[]> GetVehicles()
		{
			throw new NotImplementedException();
		}

		public Task<VehicleStatus> GetVehicleStatus(string vin)
		{
			throw new NotImplementedException();
		}

		public Task<bool> LockDoors(string vin)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UnlockDoors(string vin)
		{
			throw new NotImplementedException();
		}
	}
}

