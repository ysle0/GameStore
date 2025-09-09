using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this WebApplication app)
    {
        app.Logger.LogInformation(17, "Initializing database...");

        await app.MigrateDbAsync();
        await app.SeedDbAsync();

        app.Logger.LogInformation(18, "Database initialized.");
    }

    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        GameStoreContext dbCtx = scope.ServiceProvider
            .GetRequiredService<GameStoreContext>();

        try
        {
            await dbCtx.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while migrating the database." + ex.Message);
        }
    }

    public static async Task SeedDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        GameStoreContext dbCtx = scope.ServiceProvider
            .GetRequiredService<GameStoreContext>();

        try
        {
            bool isDbEmpty = await dbCtx.Games.AnyAsync();
            if (!isDbEmpty)
            {
                await dbCtx.Genres.AddRangeAsync(
                    new Genre { Name = "Fighting" },
                    new Genre { Name = "Kids And Family", },
                    new Genre { Name = "Racing", },
                    new Genre { Name = "Roleplaying", },
                    new Genre { Name = "Sports", }
                );

                await dbCtx.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
        }
    }
}
