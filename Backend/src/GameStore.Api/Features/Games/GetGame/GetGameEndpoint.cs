using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using AppContext = GameStore.Api.Data.AppContext;

namespace GameStore.Api.Features.Games.GetGame;

public static class GetGameEndpoint {
    public static void MapGetGame(this IEndpointRouteBuilder app) {
        app.MapGet("/{id:guid}", (Guid id, AppContext dbCtx) => {
            Game? foundGame = dbCtx.Games.Find(id);
            if (foundGame is null) {
                return Results.NotFound();
            }

            var dto = GameDetailDto.FromGame(foundGame);
            return Results.Ok(dto);
        }).WithName(EndpointNames.GetGame);
    }
}
