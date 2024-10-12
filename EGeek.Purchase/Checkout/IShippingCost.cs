using EGeek.Catalog.Contract;

namespace EGeek.Purchase.Checkout;

public interface IShippingCost
{
    Task<decimal> GetCost(List<GetProductResponse> products, string zipCode);
}