using MauiApp1.src.Core.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MauiApp1.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://schoolmuai-api.onrender.com/"); // CAMBIA POR TU IP
        }

        public async Task<string?> Login(string email, string password)
        {
            var data = new
            {
                email = email,
                password = password
            };

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/auth/login", content);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<LoginResponse> LoginUser(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }


        public async Task<bool> RegisterUser(RegisterRequest data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/auth/register", content);

            return response.IsSuccessStatusCode;
        }
        public async Task<List<Alumno>> GetAlumnos()
        {
            var response = await _httpClient.GetAsync("api/Alumnos");

            if (!response.IsSuccessStatusCode)
                return new List<Alumno>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<Alumno>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

    }
}
