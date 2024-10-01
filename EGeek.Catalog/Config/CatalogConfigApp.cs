using Microsoft.AspNetCore.Builder;

namespace EGeek.Catalog.Config;

public static class CatalogConfigApp
{
    public static void Apply(WebApplication app)
    {
        app.MapPost("/v1/catalog/products", PostProductUseCase.Action);
        app.MapPatch("/v1/catalog/products/{id}/stock", PatchStockUseCase.Action);
    }
}