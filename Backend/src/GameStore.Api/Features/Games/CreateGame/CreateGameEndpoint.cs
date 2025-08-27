using GameStore.Api.Models;
using GameStore.Api.Features.Games.Constants;
using AppContext = GameStore.Api.Data.AppContext;

namespace GameStore.Api.Features.Games.CreateGame;

public static class CreateGameEndpoint {
    public static void MapCreateGame(this IEndpointRouteBuilder app) {
        app.MapPost("/", (
            CreateNewGameDto gameDto,
            AppContext dbCtx
        ) => {
            var newGame = new Game {
                Name = gameDto.Name,
                GenreId = gameDto.GenreId,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                Description = gameDto.Description
            };

            dbCtx.Games.Add(newGame);
            dbCtx.SaveChanges();

            var output = GameDetailDto.FromGame(newGame);

            return Results.CreatedAtRoute(
                EndpointNames.GetGame,
                new { id = newGame.Id },
                output
            );
        }).WithParameterValidation();
    }
}
