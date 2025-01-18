using EGeek.Order.PurchaseOrders;
using Microsoft.EntityFrameworkCore;

namespace EGeek.Order.Infra.Database;

internal class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<PurchaseOrder> Orders { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("Order");
    }
}