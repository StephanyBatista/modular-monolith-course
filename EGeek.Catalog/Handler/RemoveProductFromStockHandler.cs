using EGeek.Catalog.Contract;
using EGeek.Catalog.Infra;
using MediatR;

namespace EGeek.Catalog.Handler;

internal class RemoveProductFromStockHandler(CatalogDbContext context)
    : IRequestHandler<RemoveProductFromStockCommand>
{
    public async Task Handle(RemoveProductFromStockCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.FindAsync(request.Id);
        product.RemoveQuantityInStock(request.Quantity, $"Shopping Cart {request.ShoppingCartId}");
        
        await context.SaveChangesAsync(cancellationToken);
    }
}