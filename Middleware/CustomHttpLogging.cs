using System.Diagnostics;
using System.Net;

namespace az_container_app.Middleware;

public class CustomHttpLogging(RequestDelegate next, ILogger<CustomHttpLogging> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation("Request: {RequestMethod}: {RequestUrl}, QueryString: {QueryString}",
            context.Request.Method, context.Request.Path, context.Request.QueryString);

        try
        {
            using (logger.BeginScope(new List<KeyValuePair<string, object>>
                   {
                       new("TraceId", Activity.Current?.Id ?? context.TraceIdentifier),
                   }))
            {
                await next(context);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        logger.LogInformation("Response: {StatusCode}", context.Response.StatusCode);
    }
}

public static class HttpLoggingExtensions
{
    public static IApplicationBuilder UseCustomHttpLogging(this IApplicationBuilder builder) => builder.UseMiddleware<CustomHttpLogging>();
}