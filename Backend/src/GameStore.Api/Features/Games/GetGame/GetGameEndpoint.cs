using System.Diagnostics;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using Microsoft.Data.Sqlite;

namespace GameStore.Api.Features.Games.GetGame;

public static class GetGameEndpoint
{
    public static void MapGetGame(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:guid}", async (
            Guid id,
            GameStoreContext dbCtx,
            ILogger<Program> logger) =>
        {
            try
            {
                Game? foundGame = await FindGameAsync(id, dbCtx);
                if (foundGame is null)
                {
                    return Results.NotFound();
                }

                var dto = GameDetailDto.FromGame(foundGame);
                return Results.Ok(dto);
            }
            catch (Exception e)
            {
                var traceId = Activity.Current?.TraceId;
                logger.LogError(e, "Error while getting game with id {Machine}. TraceId: {TraceId}",
                    Environment.MachineName,
                    traceId);

                return Results.Problem(
                    title: "An error occurred while processing your request",
                    statusCode: StatusCodes.Status500InternalServerError,
                    extensions: new Dictionary<string, object?>()
                    {
                        { "traceId", traceId?.ToString() ?? ""},
                    }
                );
            }
        }).WithName(EndpointNames.GetGame);
    }

    private static async Task<Game>? FindGameAsync(
        Guid id,
         GameStoreContext dbCtx)
    {
        throw new SqliteException("The database is not available!", 14);
        return await dbCtx.Games.FindAsync(id);
    }
}
