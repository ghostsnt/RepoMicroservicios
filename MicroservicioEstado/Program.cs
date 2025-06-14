using MicroservicioEstado.Data;
using Microsoft.EntityFrameworkCore;
using MicroservicioEstado.Hubs;

namespace MicroservicioEstado
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddSignalR();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins("http://127.0.0.1:5500") // <- Reemplaza por el puerto desde donde cargas el HTML
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // <- Esto es clave para SignalR
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowFrontend");

            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<UserStatusHub>("/userStatusHub");

            app.Run();
        }
    }
}