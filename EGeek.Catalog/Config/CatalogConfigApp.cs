using EGeek.Catalog.Products;
using Microsoft.AspNetCore.Builder;

namespace EGeek.Catalog.Config;

public static class CatalogConfigApp
{
    public static void Apply(WebApplication app)
    {
        app.MapPost("/v1/catalog/products", PostProductUseCase.Action);
        app.MapGet("/v1/catalog/products/{id}", GetProductUseCase.Action);
        app.MapGet("/v1/catalog/products", GetAllProductsUseCase.Action);
        app.MapPatch("/v1/catalog/products/{id}/stock", PatchStockUseCase.Action);
        app.MapPatch("/v1/catalog/products/{id}/price", PatchPriceUseCase.Action);
    }
}