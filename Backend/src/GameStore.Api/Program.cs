using GameStore.Api.Data;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;
using GameStoreContext = GameStore.Api.Data.GameStoreContext;

var builder = WebApplication.CreateBuilder(args);

/*
 * What service lifetime to use for a dbContext?
 * - DbContext is designed to be used as a single Unit of Work.
 * - DbContext created --> entity changes tracked --> save changes --> dispose
 * - DB connections are expensive.
 * - DBContext is not thread-safe.
 * - Increased memory usage due to change tracking.
 *
 * - USE: Scoped service lifetime.
 * - Aligning the context lifetime to the lifetime of the request.
 * - There is only one thread executing each client request at a given time.
 * - Ensure each request gets a separate DbContext instance.
 *
 */
// builder.Services.AddDbContext<GameStoreContext>(options => {
//     options.UseSqlite(connString);
// });

var connString = builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddSqlite<GameStoreContext>(connString);
var app = builder.Build();

app.MapGames();
app.MapGenres();


await app.InitializeDbAsync();

app.Logger.LogInformation(19, "Starting GameStore.Api...");

app.Run();
