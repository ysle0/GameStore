namespace GameStore.Api.Models;

public record Game
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Genre? Genre { get; set; }
    public Guid GenreId { get; set; }
    public decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public required string Description { get; set; }
    public required string ImageUri { get; set; }
}
