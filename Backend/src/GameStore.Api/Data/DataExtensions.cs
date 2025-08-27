using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions {
    public static void InitializeDb(this WebApplication app) {
        app.MigrateDb();
        app.SeedDb();
    }

    public static void MigrateDb(this WebApplication app) {
        using var scope = app.Services.CreateScope();
        AppContext dbCtx = scope.ServiceProvider
            .GetRequiredService<AppContext>();

        dbCtx.Database.Migrate();
    }

    public static void SeedDb(this WebApplication app) {
        using var scope = app.Services.CreateScope();
        AppContext dbCtx = scope.ServiceProvider
            .GetRequiredService<AppContext>();

        bool isDbEmpty = dbCtx.Games.Any();
        if (!isDbEmpty) {
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
