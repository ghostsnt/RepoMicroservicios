using System.Net.Http.Json;

namespace MicroservicioDeteccion.Service
{
    public class UserStateClient
    {
        private readonly HttpClient _http;

        public UserStateClient(HttpClient http)
        {
            _http = http;
        }

        public async Task SendState(int idUsuario, string estado, bool isInvisible)
        {
            var body = new
            {
                idUsuario,
                estado = isInvisible ? "offline" : estado,
                estaEnLinea = !isInvisible,
                ultimaConexion = DateTime.UtcNow
            };

            try
            {
                var response = await _http.PostAsJsonAsync("/api/EstadoUsuario", body);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al enviar estado: {response.StatusCode} → {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión con microservicio de estado: {ex.Message}");
            }
        }
    }
}
