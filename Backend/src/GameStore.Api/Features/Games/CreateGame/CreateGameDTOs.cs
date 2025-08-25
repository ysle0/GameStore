using System.ComponentModel.DataAnnotations;
using GameStore.Api.Models;

namespace GameStore.Api.Features.Games.CreateGame;

public record CreateNewGameDto(
    [Required] [StringLength(50)] string Name,
    Guid GenreId,
    [Range(1, 10_000)] decimal Price,
    DateOnly ReleaseDate,
    [Required] [StringLength(500)] string Description
);

public record GameDetailDto(
    Guid Id,
    string Name,
    Guid GenreId,
    decimal Price,
    DateOnly ReleaseDate,
    string Description
) {
    public static GameDetailDto FromGame(Game game) => new(
        game.Id,
        game.Name,
        game.Genre.Id,
        game.Price,
        game.ReleaseDate,
        game.Description
    );
}
