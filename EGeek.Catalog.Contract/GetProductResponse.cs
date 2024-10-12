namespace EGeek.Catalog.Contract;

public record GetProductResponse(
    string Name, 
    int QuantityInStock, 
    decimal Price,
    int WeightInGrams,
    int HeightInCentimeters,
    int WidthInCentimeters);