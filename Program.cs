using System.Text.Json;
using az_container_app.Middleware;
using Azure.Monitor.OpenTelemetry.AspNetCore;

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
        
        // Logging
        builder.Services.AddOpenTelemetry().UseAzureMonitor(o =>
        {
            o.ConnectionString =
                "InstrumentationKey=7ea923e1-bd39-441c-be4c-018208b90c2f;IngestionEndpoint=https://australiaeast-1.in.applicationinsights.azure.com/;LiveEndpoint=https://australiaeast.livediagnostics.monitor.azure.com/;ApplicationId=f5aee606-106c-492d-b399-a06424254518";
        });
        builder.Logging.SetMinimumLevel(LogLevel.Information);
        builder.Logging.ClearProviders();
        builder.Logging.AddJsonConsole(c =>
        {
            c.IncludeScopes = true;
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