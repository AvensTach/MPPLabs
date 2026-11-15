// Services/ApiService.cs
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Frontend.Models;

namespace Frontend.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private const string TokenKey = "jwt_token";

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Set base address depending on platform
            // Using HTTP and port 5197 to connect to the Docker container
            string baseAddress = DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5197"
            : "http://localhost:5197";

            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        private async Task SetAuthorizationHeaderAsync()
        {
            var token = await SecureStorage.GetAsync(TokenKey);
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var loginData = new UserLogin { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResult != null && !string.IsNullOrEmpty(authResult.Token))
                {
                    // Store token securely
                    await SecureStorage.SetAsync(TokenKey, authResult.Token);
                    return true;
                }
            }
            return false;
        }

        public void Logout()
        {
            SecureStorage.Remove(TokenKey);
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<IEnumerable<Game>> GetGamesAsync()
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync("/api/resources");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Game>>() ?? new List<Game>();
            }
            return new List<Game>();
        }

        public async Task<bool> DeleteGameAsync(int id)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.DeleteAsync($"/api/resources/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateGameAsync(CreateGameDto game)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync("/api/resources", game);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateGameAsync(int id, CreateGameDto game)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PutAsJsonAsync($"/api/resources/{id}", game);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Publisher>> GetPublishersAsync()
        {
            var response = await _httpClient.GetAsync("/api/publishers");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Publisher>>() ?? new List<Publisher>();
            }
            return new List<Publisher>();
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            var requestBody = new { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", requestBody);

            return response.IsSuccessStatusCode;
        }
    }
}