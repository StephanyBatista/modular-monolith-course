using EGeek.Order.PurchaseOrders;

namespace EGeek.Order.Infra.Database;

internal class PurchaseOrderRepository(OrderDbContext context) : IPurchaseOrderRepository
{
    public async Task Save(PurchaseOrder product)
    {
        await context.Orders.AddAsync(product);
    }

    public List<PurchaseOrder> GetAll()
    {
        return context.Orders.ToList();
    }

    public async Task<PurchaseOrder?> GetById(string id)
    {
        return await context.Orders.FindAsync(id);
    }

    public async Task Commit()
    {
        await context.SaveChangesAsync();
    }
}