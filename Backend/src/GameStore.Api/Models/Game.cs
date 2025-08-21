using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Models;

public class Game {
    public Guid Id { get; set; }
    [Required] [Length(5, 50)] public required string Name { get; set; }
    [Length(5, 20)] public required string Genre { get; set; }
    [Range(5, 10_000)] public decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }
}
