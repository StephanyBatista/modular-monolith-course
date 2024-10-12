using EGeek.Purchase.Checkout;
using EGeek.Purchase.ShoppingCarts;
using Microsoft.AspNetCore.Builder;

namespace EGeek.Purchase.Config;

public static class PurchaseConfigApp
{
    public static void Apply(WebApplication app)
    {
        app.MapPost("/v1/purchase/shopping-cart/add-product", PostAddProductUseCase.Action);
        app.MapGet("/v1/purchase/shopping-cart", GetShoppingCartUseCase.Action);
        app.MapGet("/v1/purchase/shipping/cost/{zipCode}", GetShippingCostUseCase.Action);
    }
}