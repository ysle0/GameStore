using GameStore.Api.Data;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;
using GameStore.Api.Shared.ErrorHandler;
using Microsoft.AspNetCore.HttpLogging;
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

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHttpLogging(opt =>
{
    opt.LoggingFields =
        HttpLoggingFields.RequestMethod |
        HttpLoggingFields.RequestPath |
        HttpLoggingFields.ResponseStatusCode |
        HttpLoggingFields.Duration;
    opt.CombineLogs = true;
});

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGames();
app.MapGenres();

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

await app.InitializeDbAsync();

app.Logger.LogInformation(19, "Starting GameStore.Api...");

app.Run();
