using System.Net.Http;
using System.Text;
using System.Text.Json;
using MauiApp1.src.Core.Models;

namespace MauiApp1.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://192.168.1.109:5017/"); // CAMBIA POR TU IP
        }

        public async Task<bool> RegisterUser(object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Usuarios/register", content);

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
