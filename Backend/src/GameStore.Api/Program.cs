using GameStore.Api.Data;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;

var builder = WebApplication.CreateBuilder(args);

// register services.
builder.Services.AddSingleton<GameStoreData>();
builder.Services.AddScoped<GameDataLogger>();

var app = builder.Build();

app.MapGames();
app.MapGenres();

app.Run();
