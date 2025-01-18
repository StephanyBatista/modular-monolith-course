using EGeek.Order.Contract;
using EGeek.Order.PurchaseOrders;
using MediatR;

namespace EGeek.Order.Handler;

internal class CreateOrderHandler(IPurchaseOrderRepository repository)
    : IRequestHandler<CreateOrderCommand>
{
    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = new PurchaseOrder(request.PurchaserEmail, request.ZipCodeShipping);

        foreach (var product in request.Products)
        {
            purchaseOrder.Add(product.ProductId, product.ProductName, product.Quantity);
        }
        
        await repository.Save(purchaseOrder);
        await repository.Commit();
    }
}