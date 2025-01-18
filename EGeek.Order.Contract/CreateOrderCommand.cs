using MediatR;

namespace EGeek.Order.Contract;

public record OrderProduct(string ProductId, string ProductName, int Quantity);

public record CreateOrderCommand(string PurchaserEmail, string ZipCodeShipping, List<OrderProduct> Products)
    : IRequest;