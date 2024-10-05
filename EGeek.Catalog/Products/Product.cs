using System.ComponentModel.DataAnnotations;

namespace EGeek.Catalog.Products;

internal class Product
{
    [MaxLength(36)]
    public string Id { get; private set; }
    [MaxLength(255)]
    public string Name { get; private set; }
    [MaxLength(512)]
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int QuantityInStock { get; private set; }
    public int WeightInGrams { get; private set; }
    public int HeightInCentimeters { get; private set; }
    public int WidthInCentimeters { get; private set; }
    [MaxLength(128)]
    public string CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    [MaxLength(128)]
    public string? UpdatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public List<ChangeTracker> ChangeTrackers { get; private set; } = [];
    
    private Product() {}

    public Product(PostProductRequest request, string email)
    {
        if(string.IsNullOrEmpty(request.Name))
            throw new ArgumentException("Product name is required");
        if(string.IsNullOrEmpty(request.Description))
            throw new ArgumentException("Product description is required");
        if(request.Price <= 0)
            throw new ArgumentException("Price must be greater than zero");
        if(request.QuantityInStock < 0)
            throw new ArgumentException("QuantityInStock must be greater than or equal to zero");
        if(request.WeightInGrams < 0)
            throw new ArgumentException("WeightInGrams must be greater than zero");
        if(request.HeightInCentimeters < 0)
            throw new ArgumentException("HeightInCentimeters must be greater than zero");
        if(request.WidthInCentimeters < 0)
            throw new ArgumentException("WidthInCentimeters must be greater than zero");
        
        Id = Guid.NewGuid().ToString();
        Name = request.Name;
        Description = request.Description;
        Price = request.Price;
        QuantityInStock = request.QuantityInStock;
        WeightInGrams = request.WeightInGrams;
        HeightInCentimeters = request.HeightInCentimeters;
        WidthInCentimeters = request.WidthInCentimeters;
        CreatedBy = email;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddQuantityInStock(int quantity, string changedBy)
    {
        if(quantity < 0)
            throw new ArgumentException("Quantity must be greater than or equal to zero");
        
        QuantityInStock += quantity;
        ChangeTrackers.Add(new ChangeTracker(changedBy, QuantityInStock, null));
    }

    public void ChangePrice(decimal price, string changedBy)
    {
        if(price <= 0)
            throw new ArgumentException("Price must be greater than zero");
        
        Price = price;
        ChangeTrackers.Add(new ChangeTracker(changedBy, null, Price));
    }
}