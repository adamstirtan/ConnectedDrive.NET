using System;
using System.Net.Http.Headers;
using System.Text.Json;

using Polly;
using Polly.Retry;

using ConnectedDrive.Models;
using ConnectedDrive.DTO;

namespace ConnectedDrive
{
	public class ConnectedDrive
	{
		private readonly Account _account;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly HttpClient _httpClient = new HttpClient();

		public ConnectedDrive(Account account)
		{
			_account = account;

			_httpClient.DefaultRequestHeaders.Add("Accept-Language", "en");
			_httpClient.DefaultRequestHeaders.Add("User-Agent", Constants.UserAgent);
            _httpClient.DefaultRequestHeaders.Add("x-user-agent", Constants.UserAgentMap[_account.Region]);

            _retryPolicy = Policy
				.HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
				.RetryAsync(3, async (response, retryCount) =>
				{
					if (response.Result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					{
						string accessToken = await GetAccessTokenAsync();

						_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
					}
				});
        }

		private async Task<string> GetAccessTokenAsync()
		{
			string url = $"https://{Constants.ServerEndpoints[_account.Region]}/eadrax-ucs/v1/presentation/oauth/config";

            HttpRequestMessage authenticationSettingsRequest = new HttpRequestMessage(HttpMethod.Get, url);

            authenticationSettingsRequest.Headers.Add("ocp-apim-subscription-key", Constants.OAuthAuthorizationKeys[_account.Region]);

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

			OAuthParametersDTO authenticationParameters = new()
			{
				ClientId = authenticationSettings.ClientId,
				ResponseType = "code"
			};

            return "access_token";
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
                null => Array.Empty<Vehicle>(),
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
			HttpRequestMessage request = new HttpRequestMessage(method, url);

			request.Headers.Add("Accept", method == HttpMethod.Get ? "application/json" : "application/json;charset=utf-8");

            Guid correlationId = Guid.NewGuid();

            request.Headers.Add("x-correlation-id", correlationId.ToString());
            request.Headers.Add("bmw-correlation-id", correlationId.ToString());
            request.Headers.Add("bmw-session-id", correlationId.ToString());

			using (HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() => _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)))
			{
				response.EnsureSuccessStatusCode();

				var stream = await response.Content.ReadAsStreamAsync();

				return await JsonSerializer.DeserializeAsync<T>(stream);
			}
        }
	}
}

