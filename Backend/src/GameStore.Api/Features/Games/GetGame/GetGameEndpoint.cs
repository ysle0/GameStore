namespace GameStore.Api.Features.Games.GetGame;

public static class GetGameEndpoint
{
    public static void MapGetGame(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:guid}", async (
                Guid id,
                GameStoreContext dbCtx,
                CancellationToken ct
            ) =>
            {
                Game? foundGame = await dbCtx.Games.FindAsync(
                    [id],
                    cancellationToken: ct);
                if (foundGame is null)
                {
                    return Results.NotFound();
                }

                var dto = GameDetailDto.FromGame(foundGame);
                return Results.Ok(dto);
            })
            .WithName(EndpointNames.GetGame);
    }
}
