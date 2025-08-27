using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions {
    public static async Task MigrateDb(this WebApplication app) {
        using var scope = app.Services.CreateScope();
        GameStoreContext dbCtx = scope.ServiceProvider
            .GetRequiredService<GameStoreContext>();

        try {
            await dbCtx.Database.MigrateAsync();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
}
