using System.Text.Json;
using az_container_app.Middleware;

namespace az_container_app;

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
        builder.Logging.SetMinimumLevel(LogLevel.Information);
        builder.Logging.ClearProviders();
        builder.Logging.AddJsonConsole(c =>
        {
            c.IncludeScopes = true;
            c.JsonWriterOptions = new JsonWriterOptions()
            {
                Indented = true
            };
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseCustomHttpLogging();

        app.Run();
    }
}