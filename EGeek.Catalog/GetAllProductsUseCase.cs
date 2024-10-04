using System.Security.Claims;
using EGeek.Catalog.Infra;
using EGeek.Id.Contract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Catalog;

internal static class GetAllProductsUseCase
{
    public static Ok<IEnumerable<GetProductResponse>> Action(
        CatalogDbContext context)
    {
        var products = context.Products.ToList();
        var response = products.Select(product => new GetProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.QuantityInStock,
            product.WeightInGrams,
            product.HeightInCentimeters,
            product.WidthInCentimeters
        ));

        return TypedResults.Ok(response);
    }
}