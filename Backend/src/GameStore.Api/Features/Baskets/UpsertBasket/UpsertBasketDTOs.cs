namespace GameStore.Api.Features.Baskets.UpsertBasket;

public record UpsertBasketDto(IEnumerable<UpsertBasketItemDto> Items);

public record UpsertBasketItemDto(Guid Id, int Quantity);
