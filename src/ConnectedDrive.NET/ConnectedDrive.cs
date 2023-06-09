using System;
using System.Text.Json;

using ConnectedDrive.Models;

namespace ConnectedDrive
{
	public class ConnectedDrive
	{
		private readonly Account _account;
		private readonly HttpClient _httpClient = new HttpClient();

		public ConnectedDrive(Account account)
		{
			_account = account;

			_httpClient.DefaultRequestHeaders.Clear();
			_httpClient.DefaultRequestHeaders.Add("Accept-Language", "en");
			_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json;charset=UTF-8");
			_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_account.AccessToken}");
			_httpClient.DefaultRequestHeaders.Add("User-Agent", Constants.UserAgent);
            _httpClient.DefaultRequestHeaders.Add("x-user-agent", Constants.UserAgentMap[_account.Region]);
        }

		public async Task<Vehicle[]> GetVehicles()
		{
            string parameters =
				$"apptimezone=${120}&appDateTime=${DateTimeOffset.Now.ToUnixTimeMilliseconds()}&tireGuardMode=ENABLED";

            string url =
				$"https://{Constants.ServerEndpoints[_account.Region]}${Constants.GetVehiclesUrl}?${parameters}";

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
			HttpRequestMessage request = new HttpRequestMessage();

			request.Headers.Add("Accept", method == HttpMethod.Get ? "application/json" : "application/json;charset=utf-8");

            Guid correlationId = Guid.NewGuid();

            request.Headers.Add("x-correlation-id", correlationId.ToString());
            request.Headers.Add("bmw-correlation-id", correlationId.ToString());
            request.Headers.Add("bmw-session-id", correlationId.ToString());

            using (HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<T>(stream);
            }
        }
	}
}

