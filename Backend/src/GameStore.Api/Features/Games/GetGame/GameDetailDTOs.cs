using GameStore.Api.Models;

namespace GameStore.Api.Features.Games.GetGame;

public record GameDetailDto(
    Guid Id,
    string Name,
    Guid GenreId,
    decimal Price,
    DateOnly ReleaseDate,
    string Description,
    string LastUpdatedBy
)
{
    public static GameDetailDto FromGame(Game game) => new(
        game.Id,
        game.Name,
        game.GenreId,
        game.Price,
        game.ReleaseDate,
        game.Description,
        game.LastUpdatedBy
    );
}
