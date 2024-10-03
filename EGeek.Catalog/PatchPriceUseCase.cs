using System.Security.Claims;
using EGeek.Id.Contract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Catalog;

internal static class PatchPriceUseCase
{
    [Authorize]
    public static async Task<Results<Ok, UnauthorizedHttpResult, NotFound>> Action(
        string id,
        PatchPriceRequest request,
        ClaimsPrincipal principal,
        CatalogDbContext context,
        RoleValidator roleValidator)
    {
        if (!await roleValidator.Validate(principal))
            return TypedResults.Unauthorized();

        var product = await context.Products.FindAsync(id);
        if (product == null)
            return TypedResults.NotFound();
        product.ChangePrice(request.Price);
        await context.SaveChangesAsync();
        
        return TypedResults.Ok();
    }
}