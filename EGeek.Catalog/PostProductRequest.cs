namespace EGeek.Catalog;

public record PostProductRequest(
    string Name, string Description, decimal Price, int QuantityInStock, 
    int WeightInGrams, int HeightInCentimeters, int WidthInCentimeters);