namespace EGeek.Catalog;

public record GetProductResponse(
    string Id, string Name, string Description, 
    decimal Price, int QuantityInStock, 
    int WeightInGrams, int HeightInCentimeters, int WidthInCentimeters);