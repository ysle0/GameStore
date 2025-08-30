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
            Game? foundGame = await dbCtx.Games.FindAsync(id);
            if (foundGame is null)
            {
                return Results.NotFound();
            }

            var dto = GameDetailDto.FromGame(foundGame);
            return Results.Ok(dto);
        }).WithName(EndpointNames.GetGame);
    }
}
