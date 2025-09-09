using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Shared.Authorization;
using Microsoft.EntityFrameworkCore;
using GameStoreContext = GameStore.Api.Data.GameStoreContext;

namespace GameStore.Api.Features.Games.DeleteGame;

public static class DeleteGameEndpoint
{
    public static void MapDeleteGame(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id:guid}", async (
                Guid id,
                GameStoreContext dbCtx,
                CancellationToken ct) =>
            {
                await dbCtx.Games
                    .Where(g => g.Id == id)
                    .ExecuteDeleteAsync(cancellationToken: ct);

                return Results.NoContent();
            })
            .WithName(EndpointNames.DeleteGame)
            .RequireAuthorization(Policies.AdminAccess);
    }
}
