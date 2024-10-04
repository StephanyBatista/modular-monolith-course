using System.ComponentModel.DataAnnotations;

namespace EGeek.Catalog;

public class ChangeTracker
{
    public int Id { get; private set; }
    [MaxLength(128)]
    public string ChangedBy { get; private set; }
    public DateTime ChangedAt { get; private set; }
    public int? NewStock { get; set; }
    public decimal? NewPrice { get; set; }

    private ChangeTracker() {}

    public ChangeTracker(string changedBy, int? newStock, decimal? newPrice)
    {
        if (string.IsNullOrWhiteSpace(changedBy))
            throw new ArgumentException("ChangedBy is required");
        if (!newStock.HasValue && !newPrice.HasValue)
            throw new ArgumentException("NewStock or NewPrice is required");
        
        ChangedBy = changedBy;
        ChangedAt = DateTime.UtcNow;
        NewStock = newStock;
        NewPrice = newPrice;
    }
}