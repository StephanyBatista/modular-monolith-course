using System.Security.Claims;
using EGeek.Catalog.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Catalog.Products;

internal static class PatchStockUseCase
{
    [Authorize]
    public static async Task<Results<Ok, UnauthorizedHttpResult, NotFound>> Action(
        string id,
        PatchStockRequest request,
        ClaimsPrincipal principal,
        CatalogDbContext context,
        RoleValidator roleValidator)
    {
        if (!await roleValidator.Validate(principal))
            return TypedResults.Unauthorized();

        var product = await context.Products.FindAsync(id);
        if (product == null)
            return TypedResults.NotFound();
        
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        product.AddQuantityInStock(request.QuantityInStock, email!);
        await context.SaveChangesAsync();
        
        return TypedResults.Ok();
    }
}