namespace GameStore.Api.Features.Games.GetGames;

public record GetGameDto(
    Guid Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);
