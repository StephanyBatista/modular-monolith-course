namespace EGeek.Catalog.Products;

internal record GetProductResponse(
    string Id, string Name, string Description, 
    decimal Price, int QuantityInStock, 
    int WeightInGrams, int HeightInCentimeters, int WidthInCentimeters);