using System;
using System.Text.Json.Serialization;

namespace ConnectedDrive.DTO
{
    public class OAuthParametersDTO
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = string.Empty;

        [JsonPropertyName("response_type")]
        public string? ResponseType { get; set; }

        [JsonPropertyName("redirect_uri")]
        public string? RedirectUri { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("nonce")]
        public string? Nonce { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("code_challenge")]
        public string? CodeChallenge { get; set; }

        [JsonPropertyName("code_challenge_method")]
        public string? CodeChallengeMethod { get; set; }
    }
}