using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Catalog;

internal static class PostProductUseCase
{
    [Authorize]
    public static async Task<Created<string>> Action(
        PostProductRequest request,
        ClaimsPrincipal principal,
        CatalogDbContext context)
    {
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        var product = new Product(request, email);
        context.Products.Add(product);
        await context.SaveChangesAsync();
        
        return TypedResults.Created($"/v1/catalog/products/{product.Id}", product.Id);
    }
}