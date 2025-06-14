using MicroservicioEstado.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroservicioEstado.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserStatus> EstadosUsuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserStatus>()
                .ToTable("EstadoUsuario")
                .HasKey(e => e.IdEstado);

            // Mapeo explícito de propiedades si lo deseas (opcional si usas atributos)
            modelBuilder.Entity<UserStatus>().Property(e => e.IdUsuario).IsRequired();
            modelBuilder.Entity<UserStatus>().Property(e => e.Estado).HasMaxLength(20).IsRequired();
            //modelBuilder.Entity<UserStatus>().Property(e => e.EstaEnLinea).HasMaxLength(1).IsRequired();
        }
    }
}
