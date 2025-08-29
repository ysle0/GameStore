using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static void InitializeDb(this WebApplication app)
    {
        app.MigrateDb();
        app.SeedDb();
    }

    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        GameStoreContext dbCtx = scope.ServiceProvider
            .GetRequiredService<GameStoreContext>();

        dbCtx.Database.Migrate();
    }

    public static void SeedDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        GameStoreContext dbCtx = scope.ServiceProvider
            .GetRequiredService<GameStoreContext>();

        bool isDbEmpty = dbCtx.Games.Any();
        if (!isDbEmpty)
        {
            dbCtx.Genres.AddRange(
                new Genre { Name = "Fighting" },
                new Genre { Name = "Kids And Family", },
                new Genre { Name = "Racing", },
                new Genre { Name = "Roleplaying", },
                new Genre { Name = "Sports", }
            );

            dbCtx.SaveChanges();
        }
    }
}
