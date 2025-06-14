using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroservicioEstado.Data;
using MicroservicioEstado.Models;
using Microsoft.AspNetCore.SignalR;
using MicroservicioEstado.Hubs;

namespace MicroservicioEstado.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoUsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<UserStatusHub> _hubContext;

        public EstadoUsuarioController(AppDbContext context, IHubContext<UserStatusHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: api/EstadoUsuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserStatus>>> GetEstados()
        {
            return await _context.EstadosUsuarios
                //.Where(e => e.EstadoFila == "A")
                .ToListAsync();
        }

        // GET: api/EstadoUsuario/5
        [HttpGet("{idUsuario}")]
        public async Task<ActionResult<UserStatus>> GetEstadoPorUsuario(int idUsuario)
        {
            var estado = await _context.EstadosUsuarios
                .Where(e => e.IdUsuario == idUsuario)
                .OrderByDescending(e => e.FechaActualizacion)
                .FirstOrDefaultAsync();

            if (estado == null)
                return NotFound();

            return estado;
        }

        // POST: api/EstadoUsuario
        [HttpPost]
        public async Task<IActionResult> RegistrarOActualizarEstado([FromBody] UserStatus nuevoEstado)
        {
            try
            {


                var estadoExistente = await _context.EstadosUsuarios
                    .FirstOrDefaultAsync(e => e.IdUsuario == nuevoEstado.IdUsuario);

                if (estadoExistente != null)
                {
                    estadoExistente.EstaEnLinea = nuevoEstado.EstaEnLinea;
                    estadoExistente.Estado = nuevoEstado.Estado;
                    estadoExistente.UltimaConexion = DateTime.UtcNow;
                    estadoExistente.FechaActualizacion = DateTime.UtcNow;
                }
                else
                {
                    nuevoEstado.FechaRegistro = DateTime.UtcNow;
                    nuevoEstado.FechaActualizacion = DateTime.UtcNow;
                    nuevoEstado.UltimaConexion = DateTime.UtcNow;
                    //nuevoEstado.EstadoFila = "A";

                    _context.EstadosUsuarios.Add(nuevoEstado);
                }

                await _context.SaveChangesAsync();

                // Broadcast vía SignalR (puedes incluir nombre desde otra tabla si lo necesitas)
                await _hubContext.Clients.All.SendAsync("ReceiveStatus", $"Usuario {nuevoEstado.IdUsuario}", nuevoEstado.Estado);

                return Ok(nuevoEstado);
            }
            catch(Exception ex)
            {
                Console.WriteLine("❌ ERROR EN POST EstadoUsuario: " + ex.Message);
                return StatusCode(500, "Error interno: " + ex.Message);
            }
        }
    }
}
