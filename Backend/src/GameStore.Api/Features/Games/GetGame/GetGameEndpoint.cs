using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;

namespace GameStore.Api.Features.Games.GetGame;

public static class GetGameEndpoint {
    public static void MapGetGame(
        this IEndpointRouteBuilder app,
        GameStoreData data
    ) {
        app.MapGet("/{id:guid}", (Guid id) => {
            Game? game = data.GetGameById(id);
            if (game is null) {
                return Results.NotFound();
            }

            var dto = GameDetailDto.FromGame(game);
            return Results.Ok(dto);
        }).WithName(EndpointNames.GetGame);
    }
}
