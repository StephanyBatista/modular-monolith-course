using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Purchase.ShoppingCarts;

internal static class GetShoppingCartUseCase
{
    [Authorize]
    public static async Task<Results<Ok<GetShoppingCartResponse>, NotFound>> Action(
        ClaimsPrincipal principal,
        IShoppingCartRepository repository)
    {
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        var cart = repository.GetBy(email, Status.Pending);
        
        if (cart == null || cart.Id == 0)
            return TypedResults.NotFound();

        var response = new GetShoppingCartResponse(cart);
        
        return TypedResults.Ok(response);
    }
}