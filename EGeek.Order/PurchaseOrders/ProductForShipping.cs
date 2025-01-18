using System.ComponentModel.DataAnnotations;

namespace EGeek.Order.PurchaseOrders;

internal class ProductForShipping {
    public ProductForShipping(string productId, string productName, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
    }
    
    internal ProductForShipping() {}

    public int Id { get; private set; }
    [MaxLength(36)]
    public string ProductId { get; private set; }
    [MaxLength(128)]
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
}