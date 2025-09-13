namespace GameStore.Api.Features.Games.UpdateGame;

public record UpdateGameDto(
    [Required][StringLength(50)] string Name,
    Guid GenreId,
    [Range(1, 10_000)] decimal Price,
    DateOnly ReleaseDate,
    [Required][StringLength(500)] string Description
)
{
    public IFormFile? ImageFile { get; set; }
}

