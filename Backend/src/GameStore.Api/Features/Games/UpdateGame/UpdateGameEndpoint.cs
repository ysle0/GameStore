using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using GameStore.Api.Shared.Authorization;
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
                ClaimsPrincipal? user,
                CancellationToken ct
            ) =>
            {
                string? currentUserId = user?.FindFirstValue(JwtRegisteredClaimNames.Sub);
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Results.Unauthorized();
                }


                Game? existingGame = await dbCtx.Games.FindAsync([id], cancellationToken: ct);
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
                existingGame.LastUpdatedBy = currentUserId;

                await dbCtx.SaveChangesAsync(ct);

                return Results.NoContent();
            })
            .WithParameterValidation()
            .DisableAntiforgery()
            .RequireAuthorization(Policies.AdminAccess);
    }
}
