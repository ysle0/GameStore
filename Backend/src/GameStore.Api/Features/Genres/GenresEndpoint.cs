using GameStore.Api.Data;
using GameStore.Api.Features.Genres.GetGenre;

namespace GameStore.Api.Features.Genres;

public static class GenresEndpoint {
    public static void MapGenres(this IEndpointRouteBuilder app) {
        var group = app.MapGroup("/genres");
        group.MapGetGenre();
    }
}
