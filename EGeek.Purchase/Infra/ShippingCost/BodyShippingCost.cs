namespace EGeek.Purchase.ShippingCost;

internal record BodyShippingCost(string zipCode, List<BodyItem> products);