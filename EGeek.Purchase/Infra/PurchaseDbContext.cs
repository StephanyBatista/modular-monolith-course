using EGeek.Purchase.ShoppingCarts;
using Microsoft.EntityFrameworkCore;

namespace EGeek.Purchase;

internal class PurchaseDbContext(DbContextOptions<PurchaseDbContext> options) : DbContext(options)
{
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("purchase");
    }
}