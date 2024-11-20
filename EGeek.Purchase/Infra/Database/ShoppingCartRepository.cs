using EGeek.Purchase.ShoppingCarts;
using Microsoft.EntityFrameworkCore;

namespace EGeek.Purchase.Infra.Database;

internal class ShoppingCartRepository(PurchaseDbContext context) : IShoppingCartRepository
{
    public async Task Save(ShoppingCart shoppingCart)
    {
        await context.ShoppingCarts.AddAsync(shoppingCart);
    }

    public ShoppingCart? GetBy(string email, Status status)
    {
        return context.ShoppingCarts
            .Include(s => s.Items)
            .FirstOrDefault(p => p.Email == email && p.Status == status);
    }

    public async Task Commit()
    {
        await context.SaveChangesAsync();
    }
}