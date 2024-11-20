using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Catalog.Products;

internal static class GetProductUseCase
{
    public static async Task<Results<Ok<GetProductResponse>, NotFound>> Action(
        string id,
        IProductRepository repository)
    {
        var product = await repository.GetById(id);
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