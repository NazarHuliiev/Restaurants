using System.Diagnostics;

namespace Restaurants.API.Middlewares;

public class RequestDurationLoggerMiddleware(ILogger<RequestDurationLoggerMiddleware> logger) : IMiddleware
{
    private const int _msForLog = 4000;
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var sw = Stopwatch.StartNew();
        
        await next(context);

        sw.Stop();
        
        if (sw.ElapsedMilliseconds > _msForLog)
        {
            logger.LogWarning(
                "Request [{Verb}] at {Path} took {Time} ms",
                context.Request.Method,
                context.Request.Path,
                sw.ElapsedMilliseconds);
        }
    }
}