using GameStore.Api.Models;

const string GetGameEndPointName = "GetGame";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Game> games =
[
    new Game {
        Id = Guid.NewGuid(),
        Name = "Street Fighter 11",
        Genre = "Action",
        Price = 19.99m,
        ReleaseDate = new DateOnly(1992, 7, 15)
    },
    new Game {
        Id = Guid.NewGuid(),
        Name = "Final Fantasy XIV",
        Genre = "Roleplaying",
        Price = 59.99m,
        ReleaseDate = new DateOnly(2010, 9, 30)
    },
    new Game {
        Id = Guid.NewGuid(),
        Name = "FIFA 23",
        Genre = "Sports",
        Price = 69.99m,
        ReleaseDate = new DateOnly(2022, 9, 27)
    }
];

app.MapGet("/", () => "Welcome to the Game Store API!");

app.MapGet("/games", () => games);

app.MapGet("/games/{id}", (Guid id) =>
{
    Game? game = games.Find(g => g.Id == id);

    if (game is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(game);
})
.WithName(GetGameEndPointName);

app.MapPost("/games", (Game game) =>
{
    game.Id = Guid.NewGuid();
    games.Add(game);
    return Results.CreatedAtRoute(
        GetGameEndPointName,
        new { id = game.Id },
        game);
});

app.Run();

