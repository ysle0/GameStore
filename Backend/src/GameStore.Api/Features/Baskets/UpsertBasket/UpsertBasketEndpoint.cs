using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Baskets.UpsertBasket;

public static class UpsertBasketEndpoint
{
    public static void MapUpsertBasket(this IEndpointRouteBuilder app)
    {
        // PUT /baskets/{id}
        app.MapPut("/{userId:guid}", async (
                Guid userId,
                UpsertBasketDto upsertBasketDto,
                GameStoreContext dbCtx,
                CancellationToken ct) =>
            {
                var basket = await dbCtx
                    .CustomerBaskets
                    .Include(b => b.Items)
                    .FirstOrDefaultAsync(b => b.Id == userId, cancellationToken: ct);

                if (basket is null)
                {
                    basket = new CustomerBasket
                    {
                        Id = userId,
                        Items = upsertBasketDto.Items
                            .Select(i => new BasketItem
                            {
                                GameId = i.Id,
                                Quantity = i.Quantity
                            })
                            .ToList(),
                    };

                    dbCtx.CustomerBaskets.Add(basket);
                }
                else
                {
                    basket.Items = upsertBasketDto.Items
                        .Select(i => new BasketItem
                        {
                            GameId = i.Id,
                            Quantity = i.Quantity
                        })
                        .ToList();
                }

                await dbCtx.SaveChangesAsync(ct);

                return Results.NoContent();
            })
            .WithName(EndpointNames.UpsertBasket);
    }
}
