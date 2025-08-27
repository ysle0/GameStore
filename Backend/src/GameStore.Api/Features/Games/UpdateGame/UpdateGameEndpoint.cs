using GameStore.Api.Models;
using AppContext = GameStore.Api.Data.AppContext;

namespace GameStore.Api.Features.Games.UpdateGame;

public static class UpdateGameEndpoint {
    public static void MapUpdateGame(this IEndpointRouteBuilder app) {
        app.MapPut("/{id:guid}", (
            Guid id,
            UpdateGameDto gameDto,
            AppContext dbCtx
        ) => {
            Game? existingGame = dbCtx.Games.Find(id);
            if (existingGame is null) {
                return Results.NotFound();
            }

            existingGame.Name = gameDto.Name;
            existingGame.GenreId = gameDto.GenreId;
            existingGame.Price = gameDto.Price;
            existingGame.ReleaseDate = gameDto.ReleaseDate;
            existingGame.Description = gameDto.Description;
            
            dbCtx.SaveChanges();

            return Results.NoContent();
        }).WithParameterValidation();
    }
}
