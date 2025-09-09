using GameStore.Api.Features.Games.Constants;
using Microsoft.EntityFrameworkCore;
using GameStoreContext = GameStore.Api.Data.GameStoreContext;

namespace GameStore.Api.Features.Games.GetGames;

public static class GetGamesEndpoint
{
    public static void MapGetGames(this IEndpointRouteBuilder app)
    {
        _ = app.MapGet("/", static async (
                GameStoreContext dbCtx,
                [AsParameters] GetGamesDto request,
                CancellationToken ct) =>
            {
                int skipCount = (request.Page - 1) * request.Size;

                IQueryable<Models.Game> games;
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    games = dbCtx.Games;
                }
                else
                {
                    games = dbCtx.Games
                        .Where(g => EF.Functions.Like(g.Name, $"%{request.Name}%"));
                    // .Where(g => g.Name.Contains(
                    //     request.Name,
                    //     StringComparison.OrdinalIgnoreCase)
                    // );
                }

                var paginatedGames = await games
                    .AsNoTracking()
                    .OrderBy(g => g.Name)
                    .Skip(skipCount)
                    .Take(request.Size)
                    .Include(g => g.Genre)
                    .Select(game => new GameSummaryDto(
                        game.Id,
                        game.Name,
                        game.Genre!.Name,
                        game.Price,
                        game.ReleaseDate,
                        game.ImageUri,
                        game.LastUpdatedBy
                    ))
                    .ToListAsync(ct);

                int totalGames = await games.CountAsync(cancellationToken: ct);
                int totalPages = (int)Math.Ceiling(totalGames / (double)request.Size);

                return new GamesPageDto(totalPages, paginatedGames);
            })
            .WithName(EndpointNames.GetGames);
    }
}
