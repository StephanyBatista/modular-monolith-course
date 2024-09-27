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
        IMediator mediator)
    {
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        
        var query = new GetUserQuery(email);
        var result = await mediator.Send(query);

        if (result.Role != "Catalog")
            return TypedResults.Unauthorized();
        
        var product = new Product(request, email);
        context.Products.Add(product);
        await context.SaveChangesAsync();
        
        return TypedResults.Created($"/v1/catalog/products/{product.Id}", product.Id);
    }
}