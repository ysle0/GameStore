using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using GameStore.Api.Shared.FileUpload;
using Microsoft.AspNetCore.Mvc;
using GameStoreContext = GameStore.Api.Data.GameStoreContext;

namespace GameStore.Api.Features.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:guid}", async (
            Guid id,
            [FromForm] UpdateGameDto gameDto,
            GameStoreContext dbCtx,
            FileUploader fileUploader,
            CancellationToken ct
        ) =>
        {
            Game? existingGame = await dbCtx.Games.FindAsync(id);
            if (existingGame is null)
            {
                return Results.NotFound();
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

            existingGame.Name = gameDto.Name;
            existingGame.GenreId = gameDto.GenreId;
            existingGame.Price = gameDto.Price;
            existingGame.ReleaseDate = gameDto.ReleaseDate;
            existingGame.Description = gameDto.Description;
            existingGame.ImageUri = imageUri.ToString();

            await dbCtx.SaveChangesAsync(ct);

            return Results.NoContent();

        })
        .WithParameterValidation()
        .DisableAntiforgery();
    }
}
