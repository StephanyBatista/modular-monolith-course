using EGeek.Catalog.Contract;
using EGeek.Catalog.Products;
using MediatR;
using GetProductResponse = EGeek.Catalog.Contract.GetProductResponse;

namespace EGeek.Catalog.Handler;

internal class GetProductHandler(IProductRepository repository)
    : IRequestHandler<GetProductQuery, GetProductResponse?>
{
    public async Task<GetProductResponse?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.GetById(request.Id);
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