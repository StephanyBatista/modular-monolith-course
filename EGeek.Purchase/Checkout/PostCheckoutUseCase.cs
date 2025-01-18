using System.Security.Claims;
using EGeek.Catalog.Contract;
using EGeek.Order.Contract;
using EGeek.Purchase.ShoppingCarts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Purchase.Checkout;

internal static class PostCheckoutUseCase
{
    [Authorize]
    public static async Task<Ok<PostCheckoutResponse>> Action(
        PostCheckoutRequest request,
        ClaimsPrincipal principal,
        IShoppingCartRepository repository,
        IShippingCost shippingCost,
        IPayment payment,
        IMediator mediator
    )
    {
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");
        
        var cart = repository.GetBy(email, Status.Pending);
        
        if (cart == null || cart.Id == 0)
            throw new ArgumentException("Client does not have a shopping cart");
        
        var products = await GetProductsFromCatalog(mediator, cart);
        var cost = await shippingCost.GetCost(products, request.ZipCode);
        var total = cart.Total + cost;

        var (approved, reason) =
            await payment.Process(
                total,
                request.CardNumber,
                request.CardholderName,
                request.ExpirationDate,
                request.Cvv);
        
        if(!approved)
            return TypedResults.Ok(new PostCheckoutResponse(approved, reason));

        await RemoveProductsFromStock(mediator, cart);
        await CreateOrder(mediator, cart, request.ZipCode);

        cart.Finish();

        await repository.Commit();
        
        return TypedResults.Ok(new PostCheckoutResponse(true, reason));
    }

    private static async Task RemoveProductsFromStock(IMediator mediator, ShoppingCart cart)
    {
        foreach (var item in cart.Items)
        {
            var command = new RemoveProductFromStockCommand(item.ProductId, item.Quantity, cart.Id);
            await mediator.Send(command);
        }
    }
    
    private static async Task CreateOrder(IMediator mediator, ShoppingCart cart, string zipCode)
    {
        var orderProducts = new List<OrderProduct>();
        foreach (var item in cart.Items)
        {
            orderProducts.Add(new OrderProduct(item.ProductId, item.ProductName, item.Quantity));
        }
        
        var command = new CreateOrderCommand(cart.Email, zipCode, orderProducts);
        await mediator.Send(command);
    }

    private static async Task<List<GetProductResponse>> GetProductsFromCatalog(IMediator mediator, ShoppingCart cart)
    {
        var productsReponse = new List<GetProductResponse>();
        foreach (var item in cart.Items)
        {
            var query = new GetProductQuery(item.ProductId);
            var product = await mediator.Send(query);
            productsReponse.Add(product);
        }

        return productsReponse;
    }
}