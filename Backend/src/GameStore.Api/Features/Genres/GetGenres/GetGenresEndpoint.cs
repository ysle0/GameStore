namespace GameStore.Api.Features.Genres.GetGenres;

public static class GetGenreEndpoint
{
    public static void MapGetGenres(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (GameStoreContext dbCtx) =>
                await dbCtx.Genres
                    .AsNoTracking()
                    .Select(g => new GenreDto(g.Id, g.Name))
                    .ToListAsync()
            )
            .WithName(EndpointNames.GetGenres);
    }
}
