using System.Security.Claims;
using EGeek.Id.Contract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Catalog;

internal static class PostProductUseCase
{
    [Authorize]
    public static async Task<Results<Created<string>, UnauthorizedHttpResult>> Action(
        PostProductRequest request,
        ClaimsPrincipal principal,
        CatalogDbContext context,
        RoleValidator roleValidator)
    {
        if (!await roleValidator.Validate(principal))
            return TypedResults.Unauthorized();
        
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        var product = new Product(request, email!);
        context.Products.Add(product);
        await context.SaveChangesAsync();
        
        return TypedResults.Created($"/v1/catalog/products/{product.Id}", product.Id);
    }
}