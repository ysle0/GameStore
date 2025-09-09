namespace GameStore.Api.Features.Baskets.GetBasket;

public record BasketDto(
    Guid CustomerId,
    IEnumerable<BasketItemDto> Items
)
{
    public decimal TotalPriceAmount => Items.Sum(i => i.Price * i.Quantity);
}

public record BasketItemDto(
    Guid Id,
    string Name,
    decimal Price,
    int Quantity,
    string ImageUri
);
