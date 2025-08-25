using GameStore.Api.Data;
using GameStore.Api.Models;
using GameStore.Api.Features.Games.Constants;

namespace GameStore.Api.Features.Games.CreateGame;

public static class CreateGameEndpoint {
    public static void MapCreateGame(
        this IEndpointRouteBuilder app,
        GameStoreData data
    ) {
        app.MapPost("/games", (CreateNewGameDto gameDto) => {
            Genre? genre = data.GetGenreById(gameDto.GenreId);
            if (genre is null) {
                return Results.BadRequest("Invalid Genre ID");
            }

            var newGame = new Game {
                Name = gameDto.Name,
                Genre = genre,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                Description = gameDto.Description
            };

            data.AddGame(newGame);

            var output = GameDetailDto.FromGame(newGame);

            return Results.CreatedAtRoute(
                EndpointNames.GetGame,
                new { id = newGame.Id },
                output
            );
        }).WithParameterValidation();
    }
}
