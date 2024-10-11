using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EGeek.Purchase.ShoppingCarts;

internal class ShoppingCart
{
    public int Id { get; private set; }
    public string Email { get; private set; }
    public decimal Total { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<Item> Items { get; private set; } = [];

    public ShoppingCart(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException("Email is required");
        
        Email = email;
    }

    public void AddItem(string productId, string productName, int quantity, decimal price)
    {
        if(Items.Any(i => i.ProductId == productId))
            throw new ArgumentException("Product already exists");
        
        Items.Add(new Item(productId, productName, quantity, price));
    }
}