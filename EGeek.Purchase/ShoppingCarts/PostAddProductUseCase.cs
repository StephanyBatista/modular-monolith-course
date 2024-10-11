using System.Security.Claims;
using EGeek.Catalog.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Purchase.ShoppingCarts;

internal static class PostAddProductUseCase
{
    public static async Task<Ok> Action(
        PostAddProductRequest request,
        ClaimsPrincipal principal,
        PurchaseDbContext context,
        IMediator mediator)
    {
        var query = new GetProductQuery(request.ProductId);
        var result = await mediator.Send(query);
        
        if(result == null)
            throw new ArgumentException("Product not found");
        
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        var cart = await context.ShoppingCarts.FindAsync(email) ?? new ShoppingCart(email);

        cart.AddItem(request.ProductId, result.Name, request.Quantity, result.Price);
        
        if(cart.Id == 0)
            context.ShoppingCarts.Add(cart);
        
        await context.SaveChangesAsync();
        
        return TypedResults.Ok();
    }
}