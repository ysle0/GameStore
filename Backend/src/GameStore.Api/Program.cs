using GameStore.Api.Models;

const string getGameEndPointName = "GetGame";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Genre> genres = [
    new() {
        Id = new Guid("1EE90AA6-0FB2-4799-87C6-84A0C35D2510"),
        Name = "Action",
    },
    new() {
        Id = new Guid("9cd66e98-1d83-400f-96a7-fac5bfe0416f"),
        Name = "Kids And Family",
    },
    new() {
        Id = new Guid("73df4803-f510-4fff-ab75-344ac3b13eaf"),
        Name = "Racing",
    },
    new() {
        Id = new Guid("58cb3ad0-1fb0-41d6-a02a-fe3a7f9fdacb"),
        Name = "Roleplaying",
    },
    new() {
        Id = new Guid("2b54a607-1a47-42e6-b21a-ff6432d83b14"),
        Name = "Sports",
    },
];

List<Game> games = [
    new Game {
        Id = Guid.NewGuid(),
        Name = "Street Fighter II",
        Genre = genres[0],
        Price = 19.99m,
        ReleaseDate = new DateOnly(1992, 7, 15),
        Description =
            "Street Fighter II, the most popular fighting game of all time, is back! With a new look and a new fighting style, Street Fighter 11 is the ultimate fighting game for all ages.",
    },
    new Game {
        Id = Guid.NewGuid(),
        Name = "Final Fantasy XIV",
        Genre = genres[3],
        Price = 59.99m,
        ReleaseDate = new DateOnly(2010, 9, 30),
        Description =
            "Final Fantasy XIV is a 2022 action role-playing game developed by Square Enix and published by Square Enix Co., Ltd, and This game broadens the user's experience to infinity!",
    },
    new Game {
        Id = Guid.NewGuid(),
        Name = "FIFA 23",
        Genre = genres[^1],
        Price = 69.99m,
        ReleaseDate = new DateOnly(2022, 9, 27),
        Description =
            "FIFA 23 is a 2022 sports video game developed and published by EA Sports. It is the third installment in the FIFA series, following FIFA 22 and FIFA 21, be the team and take a win!",
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
    .WithName(getGameEndPointName);

app.MapPost("/games", (Game game) => {
    game.Id = Guid.NewGuid();
    games.Add(game);
    return Results.CreatedAtRoute(
        getGameEndPointName,
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
