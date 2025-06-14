using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MicroservicioEstado.Models
{
    [Table("EstadoUsuario")]
    public class UserStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEstado { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        public bool EstaEnLinea { get; set; }

        public DateTime UltimaConexion { get; set; }

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "offline"; // online, offline, ausente, invisible

        //[Required]
        //[MaxLength(1)]
        //public string EstadoFila { get; set; } = "A"; // A = activo, I = inactivo

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }
}
