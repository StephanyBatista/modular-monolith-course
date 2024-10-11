namespace EGeek.Purchase.ShoppingCarts;

internal class Item
{
    public int Id { get; private set; }
    public string ProductId { get; private set; }
    public string ProductName { get; private set; } 
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal PriceTotal { get; private set; }
    
    public Item() {}

    public Item(string productId, string productName, int quantity, decimal price)
    {
        if(string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("ProductId is required");
            
        if(string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("ProductName is required");
        
        if(quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero");
        
        if(price <= 0)
            throw new ArgumentException("Price must be greater than zero");
        
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        Price = price;
        PriceTotal = quantity * price;
    }
}