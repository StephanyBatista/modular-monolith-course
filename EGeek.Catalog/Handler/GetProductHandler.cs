using EGeek.Catalog.Contract;
using EGeek.Catalog.Infra.Database;
using MediatR;

namespace EGeek.Catalog.Handler;

internal class GetProductHandler(CatalogDbContext context)
    : IRequestHandler<GetProductQuery, GetProductResponse?>
{
    public async Task<GetProductResponse?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await context.Products.FindAsync(request.Id);
        if (product == null)
            return null;

        var response = new GetProductResponse(
            product.Name,
            product.QuantityInStock,
            product.Price,
            product.WeightInGrams,
            product.HeightInCentimeters,
            product.WidthInCentimeters
        );
        return response;
    }
}