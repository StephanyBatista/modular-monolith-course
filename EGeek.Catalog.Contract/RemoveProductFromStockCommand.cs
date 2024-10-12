using MediatR;

namespace EGeek.Catalog.Contract;

public record RemoveProductFromStockCommand(string Id, int Quantity, int ShoppingCartId)
    : IRequest;