using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Catalog.Products;

internal static class GetAllProductsUseCase
{
    public static Ok<IEnumerable<GetProductResponse>> Action(
        IProductRepository repository)
    {
        var products = repository.GetAll();
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