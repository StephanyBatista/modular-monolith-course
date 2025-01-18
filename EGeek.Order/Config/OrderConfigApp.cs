using EGeek.Order.PurchaseOrders;
using Microsoft.AspNetCore.Builder;

namespace EGeek.Order.Config;

public static class OrderConfigApp
{
    public static void Apply(WebApplication app)
    {
        app.MapGet("/v1/orders", GetAllOrdersUseCase.Action);
    }
}