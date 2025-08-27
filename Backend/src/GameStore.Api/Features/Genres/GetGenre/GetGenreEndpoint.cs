using Microsoft.EntityFrameworkCore;
using AppContext = GameStore.Api.Data.AppContext;

namespace GameStore.Api.Features.Genres.GetGenre;

public static class GetGenreEndpoint {
    public static void MapGetGenre(this IEndpointRouteBuilder app) {
        app.MapGet("/", (AppContext dbCtx) =>
            dbCtx.Genres
                .Select(g => new GenreDto(g.Id, g.Name))
                .AsNoTracking()
        );
    }
}
