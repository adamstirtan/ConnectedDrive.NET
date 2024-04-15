using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

using Microsoft.IdentityModel.Tokens;

using ConnectedDrive.Models;
using ConnectedDrive.DTO;

namespace ConnectedDrive
{
    public class ConnectedDriveClient
    {
        private readonly Account _account;
        private readonly HttpClient _httpClient = new();

        private bool _isAuthenticated;
        private string? _accessToken = string.Empty;

        public ConnectedDriveClient(Account account)
        {
            _account = account;

            _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", Constants.UserAgent);
            _httpClient.DefaultRequestHeaders.Add("X-User-Agent", Constants.UserAgentMap[_account.Region]);
            _httpClient.DefaultRequestHeaders.Add("X-Identity-Provider", Constants.IdentityProvider);
        }

        private async Task AuthenticateAsync()
        {
            string correlationId = Guid.NewGuid().ToString();
            string authenticationSettingsUrl = $"https://{Constants.ServerEndpoints[_account.Region]}/eadrax-ucs/v1/presentation/oauth/config";

            HttpRequestMessage authenticationSettingsRequest = new(HttpMethod.Get, authenticationSettingsUrl);

            authenticationSettingsRequest.Headers.Add("OCP-APIM-Subscription-Key", Constants.OAuthAuthorizationKeys[_account.Region]);
            authenticationSettingsRequest.Headers.Add("X-Correlation-Id", correlationId);
            authenticationSettingsRequest.Headers.Add("BMW-Session-Id", correlationId);
            authenticationSettingsRequest.Headers.Add("BMW-Correlation-Id", correlationId);

            AuthenticationSettingsDTO? authenticationSettings;

            using (HttpResponseMessage response = await _httpClient.SendAsync(authenticationSettingsRequest, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();

                authenticationSettings = await JsonSerializer.DeserializeAsync<AuthenticationSettingsDTO>(stream);
            }

            if (authenticationSettings is null)
            {
                throw new Exception("BMW authentication settings service is unavailable");
            }

            Random random = new();

            byte[] stateBytes = new byte[16];
            byte[] codeVerifierBytes = new byte[64];

            random.NextBytes(stateBytes);
            random.NextBytes(codeVerifierBytes);

            string codeVerifier = Base64UrlEncoder.Encode(codeVerifierBytes);
            byte[] codeVerifierBytesHashed = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));

            string codeChallenge = Base64UrlEncoder.Encode(codeVerifierBytesHashed);

            OAuthParametersDTO authenticationParameters = new()
            {
                ResponseType = "code",
                Nonce = "login_nonce",
                ClientId = authenticationSettings.ClientId,
                RedirectUri = authenticationSettings.ReturnUrl,
                State = Base64UrlEncoder.Encode(stateBytes),
                Scope = string.Join(" ", authenticationSettings.Scopes),
                CodeChallenge = codeChallenge,
                CodeChallengeMethod = "S256"
            };

            string authenticationUrl = $"{authenticationSettings.GCDMBaseUrl}/gcdm/oauth/authenticate";

            HttpRequestMessage authenticationRequest = new(HttpMethod.Post, authenticationUrl);

            authenticationRequest.Headers.Add("X-Correlation-Id", correlationId);
            authenticationRequest.Headers.Add("BMW-Session-Id", correlationId);
            authenticationRequest.Headers.Add("BMW-Correlation-Id", correlationId);

            List<KeyValuePair<string, string>> collection = [];

            collection.Add(new("grant_type", "authorization_code"));
            collection.Add(new("username", _account.UserName));
            collection.Add(new("password", _account.Password));
            collection.Add(new("client_id", authenticationParameters.ClientId));
            collection.Add(new("code_challenge", authenticationParameters.CodeChallenge));
            collection.Add(new("code_challenge_method", authenticationParameters.CodeChallengeMethod));
            collection.Add(new("nonce", authenticationParameters.Nonce));
            collection.Add(new("redirect_uri", authenticationParameters.RedirectUri));
            collection.Add(new("response_type", authenticationParameters.ResponseType));
            collection.Add(new("scope", authenticationParameters.Scope));
            collection.Add(new("state", authenticationParameters.State));

            authenticationRequest.Content = new FormUrlEncodedContent(collection);

            AuthenticationDTO? authentication;

            using (HttpResponseMessage response = await _httpClient.SendAsync(authenticationRequest, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();

                authentication = await JsonSerializer.DeserializeAsync<AuthenticationDTO>(stream);
            }

            if (authentication is null)
            {
                throw new Exception("BMW authentication settings service is unavailable");
            }

            var authorization = HttpUtility
                .ParseQueryString(authentication.RedirectTo)
                .Get("authorization") ??
                throw new Exception("BMW authorization key was not provided");

            collection.Add(new("authorization", authorization));

            HttpRequestMessage authorizationRequest = new(HttpMethod.Post, authenticationUrl.Replace("authenticate", "token"));

            authorizationRequest.Headers.Add("X-Correlation-Id", correlationId);
            authorizationRequest.Headers.Add("BMW-Session-Id", correlationId);
            authorizationRequest.Headers.Add("BMW-Correlation-Id", correlationId);

            authorizationRequest.Content = new FormUrlEncodedContent(collection);

            using (HttpResponseMessage response = await _httpClient.SendAsync(authorizationRequest, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                string accessToken = await response.Content.ReadAsStringAsync();

                _accessToken = accessToken;
                _isAuthenticated = true;
            }

            if (_isAuthenticated == false)
            {
                throw new Exception("Authentication failed.");
            }
        }

        public async Task<Vehicle[]> GetVehicles()
        {
            string parameters =
                $"apptimezone={120}&appDateTime={DateTimeOffset.Now.ToUnixTimeMilliseconds()}&tireGuardMode=ENABLED";

            string url =
                $"https://{Constants.ServerEndpoints[_account.Region]}{Constants.GetVehiclesUrl}?{parameters}";

            var vehicles = await Request<Vehicle[]>(HttpMethod.Get, url);

            return vehicles switch
            {
                null => [],
                _ => vehicles
            };
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

        private async Task<T?> Request<T>(HttpMethod method, string url)
        {
            if (!_isAuthenticated)
            {
                await AuthenticateAsync();
            }

            Console.WriteLine(_accessToken);

            HttpRequestMessage request = new(method, url);

            request.Headers.Add("Accept", method == HttpMethod.Get ? "application/json" : "application/json;charset=utf-8");

            Guid correlationId = Guid.NewGuid();

            request.Headers.Add("x-correlation-id", correlationId.ToString());
            request.Headers.Add("bmw-correlation-id", correlationId.ToString());
            request.Headers.Add("bmw-session-id", correlationId.ToString());

            using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
    }
}