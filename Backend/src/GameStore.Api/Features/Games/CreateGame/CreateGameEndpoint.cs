using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using GameStoreContext = GameStore.Api.Data.GameStoreContext;

namespace GameStore.Api.Features.Games.CreateGame;

public static class CreateGameEndpoint
{
    public static void MapCreateGame(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            CreateNewGameDto gameDto,
            GameStoreContext dbCtx
        ) =>
        {
            var newGame = new Game
            {
                Name = gameDto.Name,
                GenreId = gameDto.GenreId,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                Description = gameDto.Description
            };

            dbCtx.Games.Add(newGame);
            await dbCtx.SaveChangesAsync();

            var output = GameDetailDto.FromGame(newGame);

            return Results.CreatedAtRoute(
                EndpointNames.GetGame,
                new { id = newGame.Id },
                output
            );
        }).WithParameterValidation();
    }
}
