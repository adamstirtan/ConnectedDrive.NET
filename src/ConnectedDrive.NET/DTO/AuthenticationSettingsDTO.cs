using System;

using System.Text.Json.Serialization;

namespace ConnectedDrive.DTO
{
	public class AuthenticationSettingsDTO
	{
        [JsonPropertyName("clientName")]
        public string? ClientName { get; set; }

        [JsonPropertyName("clientSecret")]
        public string? ClientSecret { get; set; }

        [JsonPropertyName("clientId")]
        public string? ClientId { get; set; }

        [JsonPropertyName("gcdmBaseUrl")]
        public string? GCDMBaseUrl { get; set; }

        [JsonPropertyName("returnUrl")]
        public string? ReturnUrl { get; set; }

        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("authorizationEndpoint")]
        public string? AuthorizationEndpoint { get; set; }

        [JsonPropertyName("tokenEndpoint")]
        public string? TokenEndpoint { get; set; }

        [JsonPropertyName("scopes")]
        public List<string>? Scopes { get; set; }

        [JsonPropertyName("promptValues")]
        public List<string>? PromptValues { get; set; }
    }
}
