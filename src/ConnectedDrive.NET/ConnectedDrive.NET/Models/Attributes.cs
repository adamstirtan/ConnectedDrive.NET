namespace ConnectedDrive.Models;

public class Attributes
{
    public DateTimeOffset LastFetched { get; set; }

    public string? Model { get; set; }

    public int Year { get; set; }

    public int Color { get; set; }

    public string? Brand { get; set; }

    public string? DriveTrain { get; set; }

    public string? HeadUnitType { get; set; }

    public string? HeadUnityRaw { get; set; }

    public string? HMIVersion { get; set; }

    public SoftwareVersion? SoftwareVersionCurrent { get; set; }

    public SoftwareVersion? SoftwareVersionFactory { get; set; }

    public string? TelematicsUnit { get; set; }

    public string? BodyType { get; set; }

    public string? CountryOfOrigin { get; set; }

    public string? A4AType { get; set; }

    public DriverGuideInfo? DriverGuideInfo { get; set; }
}