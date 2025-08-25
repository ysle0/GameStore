using GameStore.Api.Data;

namespace GameStore.Api.Features.Genres.GetGenre;

public static class GetGenreEndpoint {
    public static void MapGetGenre(
        this IEndpointRouteBuilder app,
        GameStoreData data
    ) {
        app.MapGet("/genres", () => data
            .GetAllGenres()
            .Select(g => new GenreDto(g.Id, g.Name))
        );
    }
}
