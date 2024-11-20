using System.Security.Claims;
using EGeek.Catalog.Contract;
using EGeek.Purchase.ShoppingCarts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Purchase.Checkout;

internal record GetShippingCostResponse(decimal Cost);

internal static class GetShippingCostUseCase
{
    [Authorize]
    public static async Task<Ok<GetShippingCostResponse>> Action(
            string zipCode,
            ClaimsPrincipal principal,
            IShoppingCartRepository repository,
            IShippingCost shippingCost,
            IMediator mediator
        )
    {
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        var cart = repository.GetBy(email, Status.Pending);
        
        if (cart == null || cart.Id == 0)
            throw new ArgumentException("Client does not have a shopping cart");

        var productsReponse = new List<GetProductResponse>();

        foreach (var item in cart.Items)
        {
            var query = new GetProductQuery(item.ProductId);
            var product = await mediator.Send(query);
            productsReponse.Add(product);
        }
        
        var cost = await shippingCost.GetCost(productsReponse, zipCode);
        var response = new GetShippingCostResponse(cost);

        return TypedResults.Ok(response);
    }
}