using System.ComponentModel.DataAnnotations;

namespace EGeek.Order.PurchaseOrders;

internal class PurchaseOrder
{
    public PurchaseOrder(string purchaserEmail, string zipCodeShipping)
    {
        PurchaserEmail = purchaserEmail;
        ZipCodeShipping = zipCodeShipping;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.InProgress;
    }
    
    internal PurchaseOrder(){}

    public int Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    [MaxLength(128)]
    public string PurchaserEmail { get; private set; }
    public OrderStatus Status { get; set; }
    [MaxLength(32)]
    public string ZipCodeShipping { get; set; }
    public List<ProductForShipping> Products { get; private set; } = new List<ProductForShipping>();

    public void Add(string productId, string productName, int productQuantity)
    {
        var product = new ProductForShipping(productId, productName, productQuantity);
        Products.Add(product);
    }
}