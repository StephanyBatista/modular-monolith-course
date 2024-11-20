using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Catalog.Products;

internal static class PostProductUseCase
{
    [Authorize]
    public static async Task<Results<Created<string>, UnauthorizedHttpResult>> Action(
        PostProductRequest request,
        ClaimsPrincipal principal,
        IProductRepository repository,
        RoleValidator roleValidator)
    {
        if (!await roleValidator.Validate(principal))
            return TypedResults.Unauthorized();
        
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        var product = new Product(request, email!);
        await repository.Save(product);
        await repository.Commit();
        
        return TypedResults.Created($"/v1/catalog/products/{product.Id}", product.Id);
    }
}