namespace EGeek.Catalog.Products;

internal interface IProductRepository
{
    public Task Save(Product product);
    public List<Product> GetAll();
    public Task<Product?> GetById(string id);
    public Task Commit();
}