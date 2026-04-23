using MauiApp1.src.Core.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MauiApp1.Services
{
    public class ApiService
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://schoolmuai-api.onrender.com/")
            };
        }

        // ── Autenticación ────────────────────────────────────────────────────────

        /// <summary>
        /// Agrega Authorization: Bearer {token} al cliente compartido antes de cada request autenticado.
        /// </summary>
        private HttpClient GetAuthenticatedClient()
        {
            var token = Preferences.Get("jwt_token", string.Empty);
            _httpClient.DefaultRequestHeaders.Authorization =
                string.IsNullOrEmpty(token)
                    ? null
                    : new AuthenticationHeaderValue("Bearer", token);
            return _httpClient;
        }

        /// <summary>
        /// Lanza UnauthorizedAccessException si la respuesta es 401.
        /// Llámalo después de cualquier request autenticado.
        /// </summary>
        private static void GuardUnauthorized(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Sesión expirada");
        }

        // ── Auth endpoints ───────────────────────────────────────────────────────

        public async Task<LoginResponse?> LoginUser(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponse>(_jsonOptions);
        }

        public async Task Logout()
        {
            try
            {
                var client = GetAuthenticatedClient();
                await client.PostAsync("api/auth/logout", null);
            }
            catch { /* Ignorar errores al revocar — la sesión local ya se limpió */ }
        }

        public async Task<bool> RegisterUser(RegisterRequest data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/register", content);
            return response.IsSuccessStatusCode;
        }

        // ── Estudiante endpoints ─────────────────────────────────────────────────

        public async Task<HomeAlumnoDto?> GetHomeAlumno(int id)
        {
            var client = GetAuthenticatedClient();
            var response = await client.GetAsync($"api/estudiantes/{id}/home");

            GuardUnauthorized(response);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<HomeAlumnoDto>(_jsonOptions);
        }

        public async Task<List<HorarioDto>> GetHorario(int id)
        {
            var client = GetAuthenticatedClient();
            var response = await client.GetAsync($"api/horarios/estudiante/{id}");

            GuardUnauthorized(response);

            if (!response.IsSuccessStatusCode)
                return [];

            return await response.Content.ReadFromJsonAsync<List<HorarioDto>>(_jsonOptions) ?? [];
        }

        public async Task<List<AsistenciaDto>> GetHistorialAsistencias(int id)
        {
            var client = GetAuthenticatedClient();
            var response = await client.GetAsync($"api/asistencias/estudiante/{id}");

            GuardUnauthorized(response);

            if (!response.IsSuccessStatusCode)
                return [];

            return await response.Content.ReadFromJsonAsync<List<AsistenciaDto>>(_jsonOptions) ?? [];
        }

        public async Task<ValidarQrResponse> ValidarQR(string qrToken, int estudianteId)
        {
            var client = GetAuthenticatedClient();
            var body = new { token = qrToken, estudianteId };
            var response = await client.PostAsJsonAsync("api/qr/validar", body);

            GuardUnauthorized(response);

            // Tanto 200 como 400 pueden traer un cuerpo con el resultado
            try
            {
                var result = await response.Content.ReadFromJsonAsync<ValidarQrResponse>(_jsonOptions);
                if (result != null)
                {
                    result.Exitoso = response.IsSuccessStatusCode;
                    return result;
                }
            }
            catch { /* El body no era JSON — fallback abajo */ }

            return new ValidarQrResponse
            {
                Exitoso = response.IsSuccessStatusCode,
                Mensaje = response.IsSuccessStatusCode
                    ? "Asistencia registrada"
                    : "Error al registrar asistencia"
            };
        }

        // ── Admin endpoints (no modificados) ────────────────────────────────────

        public async Task<List<Alumno>> GetAlumnos()
        {
            var client = GetAuthenticatedClient();
            var response = await client.GetAsync("api/Alumnos");

            if (!response.IsSuccessStatusCode)
                return [];

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Alumno>>(json, _jsonOptions) ?? [];
        }
    }
}
