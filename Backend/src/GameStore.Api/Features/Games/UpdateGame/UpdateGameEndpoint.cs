using GameStore.Api.Data;
using GameStore.Api.Models;

namespace GameStore.Api.Features.Games.UpdateGame;

public static class UpdateGameEndpoint {
    public static void MapUpdateGame(this IEndpointRouteBuilder app) {
        app.MapPut("/{id:guid}", (Guid id, UpdateGameDto gameDto, GameStoreData data) => {
            Game? existingGame = data.GetGameById(id);
            if (existingGame is null) {
                return Results.BadRequest("Invalid Game ID");
            }

            Genre? genre = data.GetGenreById(gameDto.GenreId);
            if (genre is null) {
                return Results.BadRequest("Invalid Genre ID");
            }

            existingGame.Name = gameDto.Name;
            existingGame.Genre = genre;
            existingGame.GenreId = genre.Id;
            existingGame.Price = gameDto.Price;
            existingGame.ReleaseDate = gameDto.ReleaseDate;
            existingGame.Description = gameDto.Description;

            return Results.NoContent();
        }).WithParameterValidation();
    }
}
