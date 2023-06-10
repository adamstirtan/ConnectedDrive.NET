using System;

using System.Text.Json.Serialization;

namespace ConnectedDrive.DTO
{
	public class OAuthParametersDTO
	{
		[JsonPropertyName("client_id")]
		public string? ClientId { get; set; }

		[JsonPropertyName("response_type")]
		public string? ResponseType { get; set; }
	}
}

