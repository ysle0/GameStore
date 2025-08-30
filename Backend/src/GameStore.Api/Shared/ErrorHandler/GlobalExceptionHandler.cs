using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

namespace GameStore.Api.Shared.ErrorHandler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext ctx,
        Exception e,
        CancellationToken ct)
    {
        var traceId = Activity.Current?.TraceId;

        logger.LogError(e, "Error while processing request on machine: {Machine}. TraceId: {TraceId}",
            Environment.MachineName,
            traceId);

        await Results.Problem(
            title: "An error occurred while processing your request",
            statusCode: StatusCodes.Status500InternalServerError,
            extensions: new Dictionary<string, object?>()
            {
                { "traceId", traceId?.ToString() ?? ""},
            }
        ).ExecuteAsync(ctx);

        return true;
    }
}
