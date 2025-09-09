using GameStore.Api.Features.Genres.GetGenres;

namespace GameStore.Api.Features.Genres;

public static class GenresEndpoint
{
    public static void MapGenres(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/genres");
        group.MapGetGenres();
    }
}
