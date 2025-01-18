namespace EGeek.Order.PurchaseOrders;

internal interface IPurchaseOrderRepository
{
    public Task Save(PurchaseOrder product);
    public List<PurchaseOrder> GetAll();
    public Task<PurchaseOrder?> GetById(string id);
    public Task Commit();
}