using GameStore.Api.Data;

namespace GameStore.Api.Features.Games.GetGames;

public static class GetGamesEndpoint {
    public static void MapGetGames(this IEndpointRouteBuilder app) {
        app.MapGet("/", (GameStoreData data) => data
            .GetAllGames()
            .Select(g => new GetGameDto(
                g.Id,
                g.Name,
                g.Genre!.Name,
                g.Price,
                g.ReleaseDate
            ))
        );
    }
}
