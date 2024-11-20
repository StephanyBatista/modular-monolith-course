using System.Security.Claims;
using EGeek.Catalog.Contract;
using EGeek.Purchase.Infra.Database;
using EGeek.Purchase.ShoppingCarts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EGeek.Purchase.Checkout;

internal static class PostCheckoutUseCase
{
    [Authorize]
    public static async Task<Ok<PostCheckoutResponse>> Action(
        PostCheckoutRequest request,
        ClaimsPrincipal principal,
        PurchaseDbContext context,
        IShippingCost shippingCost,
        IPayment payment,
        IMediator mediator
    )
    {
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");
        
        var cart = context.ShoppingCarts
            .Include(s => s.Items)
            .FirstOrDefault(s => s.Email == email && s.Status == Status.Pending);
        
        if (cart == null || cart.Id == 0)
            throw new ArgumentException("Client does not have a shopping cart");
        
        //TODO: Refact this part
        var productsReponse = new List<GetProductResponse>();
        foreach (var item in cart.Items)
        {
            var query = new GetProductQuery(item.ProductId);
            var product = await mediator.Send(query);
            productsReponse.Add(product);
        }

        var cost = await shippingCost.GetCost(productsReponse, request.ZipCode);
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

        foreach (var item in cart.Items)
        {
            var command = new RemoveProductFromStockCommand(item.ProductId, item.Quantity, cart.Id);
            await mediator.Send(command);
        }

        cart.Finish();
        
        await context.SaveChangesAsync();
        
        return TypedResults.Ok(new PostCheckoutResponse(true, reason));
    }
}

internal record PostCheckoutResponse(bool Approved, string Reason);

internal record PostCheckoutRequest(
    string ZipCode,
    string CardNumber,
    string CardholderName,
    DateTime ExpirationDate,
    string Cvv
);