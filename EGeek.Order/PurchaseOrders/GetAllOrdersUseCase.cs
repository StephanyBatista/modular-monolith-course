using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Order.PurchaseOrders;

internal record GetOrderResponse(
    int Id,
    DateTime CreatedAt,
    string Status,
    string PurchaserEmail,
    string ZipCodeShipping);

internal class GetAllOrdersUseCase
{
    public static Ok<IEnumerable<GetOrderResponse>> Action(IPurchaseOrderRepository repository)
    {
        var orders = repository.GetAll();
        
        var response = orders.Select(
            o => new GetOrderResponse(
                o.Id,
                o.CreatedAt,
                o.Status.ToString(),
                o.PurchaserEmail,
                o.ZipCodeShipping
            ));

        return TypedResults.Ok(response);
    }
}