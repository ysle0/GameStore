
namespace GameStore.Api.Features.Games.CreateGame;

public static class CreateGameEndpoint
{
    public static void MapCreateGame(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
                ILogger<Program> logger,
                [FromForm] CreateNewGameDto gameDto,
                GameStoreContext dbCtx,
                FileUploader fileUploader,
                ClaimsPrincipal? user,
                CancellationToken ct
            ) =>
            {
                if (user?.Identity?.IsAuthenticated == false)
                {
                    return Results.Unauthorized();
                }

                string? currentUserId = user?.FindFirstValue(JwtRegisteredClaimNames.Sub);
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Results.Unauthorized();
                }

                Uri imageUri = new(FileNames.DefaultImageUri);

                IFormFile? imageFile = gameDto.ImageFile;
                if (imageFile is not null)
                {
                    FileUploadResult uploadResult = await fileUploader.UploadFileAsync(
                        imageFile,
                        StorageNames.GameImagesFolder,
                        ct);

                    if (uploadResult.IsSuccess)
                    {
                        imageUri = uploadResult.FilePath!;
                    }
                    else
                    {
                        return Results.BadRequest(new { message = uploadResult.ErrorMessage });
                    }
                }

                var newGame = new Game
                {
                    Name = gameDto.Name,
                    GenreId = gameDto.GenreId,
                    Price = gameDto.Price,
                    ReleaseDate = gameDto.ReleaseDate,
                    Description = gameDto.Description,
                    ImageUri = imageUri.ToString(),
                    LastUpdatedBy = currentUserId
                };

                dbCtx.Games.Add(newGame);
                await dbCtx.SaveChangesAsync(ct);

                logger.LogInformation(
                    "Created game {GameName} with price {GamePrice}",
                    newGame.Name,
                    newGame.Price);

                GameDetailDto gameDetail = GameDetailDto.FromGame(newGame);

                return Results.CreatedAtRoute(
                    EndpointNames.GetGame,
                    new { id = newGame.Id },
                    gameDetail
                );
            })
            .WithName(EndpointNames.CreateGame)
            .WithParameterValidation()
            .DisableAntiforgery()
            .RequireAuthorization(Policies.AdminAccess);
    }
}
