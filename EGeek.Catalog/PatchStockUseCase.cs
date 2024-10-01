using System.Security.Claims;
using EGeek.Id.Contract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Catalog;

internal static class PatchStockUseCase
{
    [Authorize]
    public static async Task<Results<Ok, UnauthorizedHttpResult, NotFound>> Action(
        string id,
        PatchStockRequest request,
        ClaimsPrincipal principal,
        CatalogDbContext context,
        IMediator mediator)
    {
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        
        var query = new GetUserQuery(email);
        var result = await mediator.Send(query);

        if (result.Role != "catalog")
            return TypedResults.Unauthorized();

        var product = await context.Products.FindAsync(id);
        if (product == null)
            return TypedResults.NotFound();
        product.AddQuantityInStock(request.QuantityInStock);
        await context.SaveChangesAsync();
        
        return TypedResults.Ok();
    }
}