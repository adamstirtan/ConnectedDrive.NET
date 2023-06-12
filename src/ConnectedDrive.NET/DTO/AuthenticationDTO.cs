using System;
using System.Text.Json.Serialization;

namespace ConnectedDrive.DTO
{
	public class AuthenticationDTO
	{
		[JsonPropertyName("redirect_to")]
		public string RedirectTo { get; set; } = string.Empty;
	}
}
