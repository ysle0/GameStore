using GameStore.Api.Models;

namespace GameStore.Api.Data;

public class GameStoreData {
    readonly List<Genre> _genres = [
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

    readonly List<Game> _games;

    public GameStoreData() {
        _games = [
            new Game {
                Id = Guid.NewGuid(),
                Name = "Street Fighter II",
                Genre = _genres[0],
                GenreId = _genres[0].Id,
                Price = 19.99m,
                ReleaseDate = new DateOnly(1992, 7, 15),
                Description =
                    "Street Fighter II, the most popular fighting game of all time, is back! With a new look and a new fighting style, Street Fighter 11 is the ultimate fighting game for all ages.",
            },
            new Game {
                Id = Guid.NewGuid(),
                Name = "Final Fantasy XIV",
                Genre = _genres[3],
                GenreId = _genres[3].Id,
                Price = 59.99m,
                ReleaseDate = new DateOnly(2010, 9, 30),
                Description =
                    "Final Fantasy XIV is a 2022 action role-playing game developed by Square Enix and published by Square Enix Co., Ltd, and This game broadens the user's experience to infinity!",
            },
            new Game {
                Id = Guid.NewGuid(),
                Name = "FIFA 23",
                Genre = _genres[^1],
                GenreId = _genres[^1].Id,
                Price = 69.99m,
                ReleaseDate = new DateOnly(2022, 9, 27),
                Description =
                    "FIFA 23 is a 2022 sports video game developed and published by EA Sports. It is the third installment in the FIFA series, following FIFA 22 and FIFA 21, be the team and take a win!",
            }
        ];
    }

    public GameStoreData(List<Game> games) {
        _games = games;
    }

    public IEnumerable<Game> GetAllGames() => _games;

    public Game? GetGameById(Guid id) => _games.Find(g => g.Id == id);

    public void AddGame(Game game) => _games.Add(game with { Id = Guid.NewGuid() });

    public void RemoveGame(Guid id) => _games.RemoveAll(g => g.Id == id);

    public IEnumerable<Genre> GetAllGenres() => _genres;

    public Genre? GetGenreById(Guid id) => _genres.Find(g => g.Id == id);
}
