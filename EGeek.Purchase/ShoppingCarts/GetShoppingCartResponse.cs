namespace EGeek.Purchase.ShoppingCarts;

internal class GetShoppingCartResponse
{
    public GetShoppingCartResponse(ShoppingCart shoppingCart)
    {
        Total = shoppingCart.Total;

        foreach (var item in shoppingCart.Items)
        {
            var itemResponse = 
                new GetItemResponse(
                    item.ProductId, item.ProductName, item.Quantity, item.Price, item.PriceTotal);
            Items.Add(itemResponse);
        }
    }

    public decimal Total { get; set; }
    public List<GetItemResponse> Items { get; set; } = [];
}

internal record GetItemResponse(
    string ProductId, string ProductName, int Quantity, decimal Price, decimal TotalPrice);