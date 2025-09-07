using System.Drawing;

namespace GameStore.Api.Features.Games.GetGames;

public record GetGamesDto(
    int Page = 1,
    int Size = 5,
    string? Name = null
);

public record GamesPageDto(
    int TotalPages,
    IEnumerable<GameSummaryDto> GameSummaries
);

public record GameSummaryDto(
    Guid Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate,
    string ImageUri,
    string LastUpdatedBy
);
