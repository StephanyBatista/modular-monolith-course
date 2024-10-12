using System.Security.Claims;
using EGeek.Catalog.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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
        
        if(request.Quantity > result.QuantityInStock)
            throw new ArgumentException("Quantity out of stock");
        
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        var cart = 
            context.ShoppingCarts
                .Include(s => s.Items)
                .FirstOrDefault(p => p.Email == email && p.Status == Status.Pending) ?? 
            new ShoppingCart(email);

        cart.AddItem(request.ProductId, result.Name, request.Quantity, result.Price);
        
        if(cart.Id == 0)
            context.ShoppingCarts.Add(cart);
        
        await context.SaveChangesAsync();
        
        return TypedResults.Ok();
    }
}