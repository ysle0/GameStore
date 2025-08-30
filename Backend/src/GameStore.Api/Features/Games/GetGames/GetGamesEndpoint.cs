using Microsoft.EntityFrameworkCore;
using GameStoreContext = GameStore.Api.Data.GameStoreContext;

namespace GameStore.Api.Features.Games.GetGames;

public static class GetGamesEndpoint
{
    public static void MapGetGames(this IEndpointRouteBuilder app)
    {
        _ = app.MapGet("/", static async (
            GameStoreContext dbCtx,
            [AsParameters] GetGamesDto request) =>
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
                .OrderBy(g => g.Name)
                .Skip(skipCount)
                .Take(request.Size)
                .Include(g => g.Genre)
                .Select(g => new GameSummaryDto(
                    g.Id,
                    g.Name,
                    g.Genre!.Name,
                    g.Price,
                    g.ReleaseDate
                ))
                .AsNoTracking()
                .ToListAsync();

            int totalGames = await games.CountAsync();
            int totalPages = (int)Math.Ceiling(
                totalGames / (double)request.Size
            );

            return new GamesPageDto(totalPages, paginatedGames);
        });
    }
}
