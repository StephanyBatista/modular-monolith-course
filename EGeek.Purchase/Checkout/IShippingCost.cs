using EGeek.Catalog.Contract;

namespace EGeek.Purchase.Checkout;

internal interface IShippingCost
{
    Task<decimal> GetCost(List<GetProductResponse> products, string zipCode);
}