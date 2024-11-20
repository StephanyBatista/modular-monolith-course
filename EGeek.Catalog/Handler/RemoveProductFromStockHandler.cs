using EGeek.Catalog.Contract;
using EGeek.Catalog.Products;
using MediatR;

namespace EGeek.Catalog.Handler;

internal class RemoveProductFromStockHandler(IProductRepository repository)
    : IRequestHandler<RemoveProductFromStockCommand>
{
    public async Task Handle(RemoveProductFromStockCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetById(request.Id);
        product.RemoveQuantityInStock(request.Quantity, $"Shopping Cart {request.ShoppingCartId}");
        
        await repository.Commit();
    }
}