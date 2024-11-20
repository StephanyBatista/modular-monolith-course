using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Catalog.Products;

internal static class PatchPriceUseCase
{
    [Authorize]
    public static async Task<Results<Ok, UnauthorizedHttpResult, NotFound>> Action(
        string id,
        PatchPriceRequest request,
        ClaimsPrincipal principal,
        IProductRepository repository,
        RoleValidator roleValidator)
    {
        if (!await roleValidator.Validate(principal))
            return TypedResults.Unauthorized();

        var product = await repository.GetById(id);
        if (product == null)
            return TypedResults.NotFound();
        
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        product.ChangePrice(request.Price, email!);

        await repository.Commit();
        
        return TypedResults.Ok();
    }
}