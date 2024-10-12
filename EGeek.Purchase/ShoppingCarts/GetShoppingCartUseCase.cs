using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EGeek.Purchase.ShoppingCarts;

internal static class GetShoppingCartUseCase
{
    [Authorize]
    public static async Task<Results<Ok<GetShoppingCartResponse>, NotFound>> Action(
        ClaimsPrincipal principal,
        PurchaseDbContext context)
    {
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        var cart =
            context.ShoppingCarts
                .Include(s => s.Items)
                .FirstOrDefault(p => p.Email == email && p.Status == Status.Pending);
        
        if (cart == null || cart.Id == 0)
            return TypedResults.NotFound();

        var response = new GetShoppingCartResponse(cart);
        
        return TypedResults.Ok(response);
    }
}