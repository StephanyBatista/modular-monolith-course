using System.ComponentModel.DataAnnotations;

namespace EGeek.Purchase.ShoppingCarts;

internal enum Status
{
    Pending,
    Finish
}

internal class ShoppingCart
{
    public int Id { get; private set; }
    [MaxLength(128)]
    public string Email { get; private set; }
    public decimal Total { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<Item> Items { get; private set; } = [];
    public Status Status { get; private set; }

    public ShoppingCart(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException("Email is required");
        
        Email = email;
        Status = Status.Pending;
        Total = 0;
    }

    public void AddItem(string productId, string productName, int quantity, decimal price)
    {
        if(Items.Any(i => i.ProductId == productId))
            throw new ArgumentException("Product already exists");
        
        Items.Add(new Item(productId, productName, quantity, price));
        Total = Items.Sum(i => i.PriceTotal);
    }
}