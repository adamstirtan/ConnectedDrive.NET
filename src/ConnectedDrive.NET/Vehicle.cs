namespace ConnectedDrive
{
	public class Vehicle
	{
		public string? VIN { get; set; }

		public string? AppVehicleType { get; set; }
	}

	public class Attributes
	{
		public DateTimeOffset LastFetched { get; set; }

		public string? Model { get; set; }

		public int Year { get; set; }

		public int Color { get; set; }

		public string? Brand { get; set; }
	}
}

