using GameStore.Api.Models;

const string GetGameEndPointName = "GetGame";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Genre> genres = [
    new() { Id = Guid.Parse("1ee90aa6-0fb2-4799-87c6-84a0c35d2510"), Name = "Action", },
    new() { Id = Guid.Parse("9cd66e98-1d83-400f-96a7-fac5bfe0416f"), Name = "Kids And Family", },
    new() { Id = Guid.Parse("73df4803-f510-4fff-ab75-344ac3b13eaf"), Name = "Racing", },
    new() { Id = Guid.Parse("58cb3ad0-1fb0-41d6-a02a-fe3a7f9fdac"), Name = "Roleplaying", },
    new() { Id = Guid.Parse("2854a607-1447-42e6-b21a-ff6432d83b1"), Name = "Sports", },
];

List<Game> games = [
    new Game {
        Id = Guid.NewGuid(),
        Name = "Street Fighter 11",
        Genre = genres[0],
        Price = 19.99m,
        ReleaseDate = new DateOnly(1992, 7, 15),
        Description = "",
    },
    new Game {
        Id = Guid.NewGuid(),
        Name = "Final Fantasy XIV",
        Genre = genres[1],
        Price = 59.99m,
        ReleaseDate = new DateOnly(2010, 9, 30),
        Description = "",
    },
    new Game {
        Id = Guid.NewGuid(),
        Name = "FIFA 23",
        Genre = genres[2],
        Price = 69.99m,
        ReleaseDate = new DateOnly(2022, 9, 27),
        Description = "",
    }
];

app.MapGet("/", () => "Welcome to the Game Store API!");

app.MapGet("/games", () => games);

app.MapGet("/games/{id}", (Guid id) => {
        Game? game = games.Find(g => g.Id == id);

        if (game is null) {
            return Results.NotFound();
        }

        return Results.Ok(game);
    })
    .WithName(GetGameEndPointName);

app.MapPost("/games", (Game game) => {
    game.Id = Guid.NewGuid();
    games.Add(game);
    return Results.CreatedAtRoute(
        GetGameEndPointName,
        new { id = game.Id },
        game);
}).WithParameterValidation();

app.MapPut("/games/{id}", (Guid id, Game updatedGame) => {
    Game? existingGame = games.Find(g => g.Id == id);
    if (existingGame is null) {
        return Results.NotFound();
    }

    existingGame.Name = updatedGame.Name;
    existingGame.Genre = updatedGame.Genre;
    existingGame.Price = updatedGame.Price;
    existingGame.ReleaseDate = updatedGame.ReleaseDate;
    int existingGameIndex = games.IndexOf(existingGame);

    games[existingGameIndex] = existingGame;

    return Results.NoContent();
}).WithParameterValidation();

app.MapDelete("/games/{id}", (Guid id) => {
    games.RemoveAll(g => g.Id == id);

    return Results.NoContent();
});

app.Run();
