using ConnectedDrive;
using ConnectedDrive.Models;

Account account = new("", "", Regions.NorthAmerica);
ConnectedDriveClient client = new(account);

var vehicles = await client.GetVehicles();

foreach (Vehicle vehicle in vehicles)
{
    Console.WriteLine(vehicle.VIN);
}