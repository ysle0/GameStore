using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Games.DeleteGame;

public static class DeleteGameEndpoint {
    public static void MapDeleteGame(this IEndpointRouteBuilder app) {
        app.MapDelete("/{id:guid}", (Guid id, GameStoreContext dbCtx) => {
            dbCtx.Games
                .Where(g => g.Id == id)
                .ExecuteDelete();
            
            return Results.NoContent();
        });
    }
}
