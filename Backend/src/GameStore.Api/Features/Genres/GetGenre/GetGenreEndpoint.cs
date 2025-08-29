using Microsoft.EntityFrameworkCore;
using GameStoreContext = GameStore.Api.Data.GameStoreContext;

namespace GameStore.Api.Features.Genres.GetGenre;

public static class GetGenreEndpoint
{
    public static void MapGetGenre(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (GameStoreContext dbCtx) =>
            await dbCtx.Genres
                .Select(g => new GenreDto(g.Id, g.Name))
                .AsNoTracking()
                .ToListAsync()
        );
    }
}
