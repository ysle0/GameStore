using System.Diagnostics;

namespace GameStore.Api.Shared;

public class RequestTimeMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext ctx,
        ILogger<RequestTimeMiddleware> logger)
    {
        Stopwatch stopwatch = new();

        try
        {
            stopwatch.Start();
            await next(ctx);
        }
        finally
        {
            stopwatch.Stop();

            long elapsedMs = stopwatch.ElapsedMilliseconds;
            logger.LogInformation("[{RequestMethod}] {RequestPath} => {statusCode} executed in {elapsed} ms",
                ctx.Request.Method,
                ctx.Request.Path,
                ctx.Response.StatusCode,
                elapsedMs);
        }
    }
}