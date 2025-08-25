using GameStore.Api.Data;
using GameStore.Api.Features.Games.CreateGame;
using GameStore.Api.Features.Games.DeleteGame;
using GameStore.Api.Features.Games.GetGame;
using GameStore.Api.Features.Games.GetGames;
using GameStore.Api.Features.Games.UpdateGame;
using GameStore.Api.Features.Genres.GetGenre;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

GameStoreData data = new();

app.MapGetGames(data);
app.MapGetGame(data);
app.MapCreateGame(data);
app.MapUpdateGame(data);
app.MapDeleteGame(data);

app.MapGetGenre(data);

app.Run();

public record GenreDto(Guid Id, string Name);
