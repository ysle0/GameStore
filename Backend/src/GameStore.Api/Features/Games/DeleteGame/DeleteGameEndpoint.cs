using Microsoft.EntityFrameworkCore;
using AppContext = GameStore.Api.Data.AppContext;

namespace GameStore.Api.Features.Games.DeleteGame;

public static class DeleteGameEndpoint {
    public static void MapDeleteGame(this IEndpointRouteBuilder app) {
        app.MapDelete("/{id:guid}", (Guid id, AppContext dbCtx) => {
            dbCtx.Games
                .Where(g => g.Id == id)
                .ExecuteDelete();
            
            return Results.NoContent();
        });
    }
}
