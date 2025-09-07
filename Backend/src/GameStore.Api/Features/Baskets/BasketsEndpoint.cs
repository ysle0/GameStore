using GameStore.Api.Features.Baskets.UpsertBasket;

namespace GameStore.Api.Features.Baskets;

public static class BasketsEndpoint
{
    public static void MapBaskets(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/baskets");
        group.MapUpsertBasket();
    }
}
