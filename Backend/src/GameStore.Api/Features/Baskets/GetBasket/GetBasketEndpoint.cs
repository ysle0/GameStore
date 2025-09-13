namespace GameStore.Api.Features.Baskets.GetBasket;

public static class GetBasketEndpoint
{
    public static void MapGetBasket(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{userId:guid}", async (
                Guid userId,
                GameStoreContext dbCtx,
                IAuthorizationService authSvc,
                ClaimsPrincipal user,
                CancellationToken ct
            ) =>
            {
                if (userId == Guid.Empty)
                {
                    return Results.BadRequest();
                }

                CustomerBasket basket = await dbCtx
                    .CustomerBaskets
                    .Include(basket => basket.Items)
                    .ThenInclude(item => item.Game)
                    .FirstOrDefaultAsync(
                        basket => basket.Id == userId,
                        cancellationToken: ct
                    ) ?? new() { Id = userId };

                var authRes = await authSvc.AuthorizeAsync(
                    user,
                    basket,
                    new OwnerOrAdminRequirement()
                );

                if (!authRes.Succeeded)
                {
                    return Results.Forbid();
                }
                
                

                var basketDto = new BasketDto(
                    CustomerId: basket.Id,
                    Items: basket.Items
                        .Select(item =>
                            new BasketItemDto(
                                item.Id,
                                item.Game!.Name,
                                item.Game!.Price,
                                item.Quantity,
                                item.Game!.ImageUri
                            )
                        )
                        .OrderBy(item => item.Name)
                );

                return Results.Ok(basketDto);
            })
            .WithName(EndpointNames.GetBasket);
    }
}
