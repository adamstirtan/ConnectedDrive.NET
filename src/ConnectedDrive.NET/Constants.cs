using ConnectedDrive.Models;

namespace ConnectedDrive
{
    public class Constants
	{
		public static readonly Dictionary<Regions, string> ServerEndpoints = new()
		{
			{ Regions.NorthAmerica, "cocoapi.bmwgroup.us" },
			{ Regions.RestOfWorld, "cocoapi.bmwgroup.com" },
			{ Regions.China, "myprofile.bmw.com.cn" }
		};

		public static readonly Dictionary<Regions, string> OAuthAuthorizationKeys = new()
		{
			{ Regions.NorthAmerica, "31e102f5-6f7e-7ef3-9044-ddce63891362" },
			{ Regions.RestOfWorld, "4f1c85a3-758f-a37d-bbb6-f8704494acfa" },
			{ Regions.China, "blF2NkNxdHhKdVhXUDc0eGYzQ0p3VUVQOjF6REh4NnVuNGNEanliTEVOTjNreWZ1bVgya0VZaWdXUGNRcGR2RFJwSUJrN3JPSg==" }
		};

		public static readonly string UserAgent = "Dart/2.14 (dart:io)";

		public static readonly Dictionary<Regions, string> UserAgentMap = new() {
			{ Regions.NorthAmerica, "android(SP1A.210812.016.C1);bmw;2.5.2(14945);na" },
			{ Regions.RestOfWorld, "android(SP1A.210812.016.C1);bmw;2.5.2(14945);row" },
			{ Regions.China, "android(SP1A.210812.016.C1);bmw;2.3.0(13603);cn" }
		};

		public static readonly string IdentityProvider = "gcdm";

		public static readonly string GetVehiclesUrl = "/eadrax-vcs/v2/vehicles";

        public static readonly string RemoteServicesUrl = "/eadrax-vrccs/v2/presentation/remote-commands";

        public static readonly string ExecuteRemoteServicesUrl = RemoteServicesUrl + "/{vehicleVin}/{serviceType}";

        public static readonly string StatusRemoteServicesUrl = RemoteServicesUrl + "/eventStatus?eventId={eventId}";

        public static readonly string StatusRemoteServicePositionUrl = RemoteServicesUrl + "/eventPosition?eventId={eventId}";
    }
}
