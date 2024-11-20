using EGeek.Catalog.Products;

namespace EGeek.Catalog.Infra.Database;

internal class ProductRepository(CatalogDbContext context) : IProductRepository
{
    public async Task Save(Product product)
    {
        await context.Products.AddAsync(product);
    }

    public List<Product> GetAll()
    {
        return context.Products.ToList();
    }

    public async Task<Product?> GetById(string id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task Commit()
    {
        await context.SaveChangesAsync();
    }
}