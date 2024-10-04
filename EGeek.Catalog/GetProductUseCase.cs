using System.Security.Claims;
using EGeek.Catalog.Infra;
using EGeek.Id.Contract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Catalog;

internal static class GetProductUseCase
{
    public static async Task<Results<Ok<GetProductResponse>, NotFound>> Action(
        string id,
        CatalogDbContext context)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
            return TypedResults.NotFound();

        var response = new GetProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.QuantityInStock,
            product.WeightInGrams,
            product.HeightInCentimeters,
            product.WidthInCentimeters
        );
        
        return TypedResults.Ok(response);
    }
}