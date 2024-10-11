using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/process-payment", (PaymentData request) =>
{
    var (approved, message) = ProcessPayment(request);
    return Results.Json(new { approved, message });
});

(bool approved, string message) ProcessPayment(PaymentData request)
{
    if (request.Amount <= 0)
    {
        return (false, "Invalid amount");
    }

    if (request.CardNumber.Length != 16)
    {
        return (false, "Invalid card number");
    }

    if (request.ExpirationDate < DateTime.Now)
    {
        return (false, "Card expired");
    }
    
    return (true, "Payment approved");
}

app.MapPost("/calculate-shipping", (ShippingData request) =>
{
    var shippingCost = CalculateShippingCost(request);
    return Results.Json(new {cost = shippingCost});
});

decimal CalculateShippingCost(ShippingData request)
{
    var baseShippingCost = 10.0m;
    
    var weightCost = request.WeightInGrams * 0.2m;
    var volume = request.HeightInCentimeters * request.WidthInCentimeters;
    var volumeCost = volume * 0.01m;
    var stateCost = GetStateShippingCost(request.State);
    
    return baseShippingCost + weightCost + volumeCost + stateCost;
}

decimal GetStateShippingCost(string state)
{
    var stateShippingCosts = new Dictionary<string, decimal>
    {
        {"AC", 25.0m}, {"AL", 15.0m}, {"AP", 25.0m}, {"AM", 30.0m}, {"BA", 15.0m},
        {"CE", 15.0m}, {"DF", 10.0m}, {"ES", 12.0m}, {"GO", 12.0m}, {"MA", 20.0m},
        {"MT", 20.0m}, {"MS", 18.0m}, {"MG", 12.0m}, {"PA", 25.0m}, {"PB", 15.0m},
        {"PR", 12.0m}, {"PE", 15.0m}, {"PI", 18.0m}, {"RJ", 10.0m}, {"RN", 15.0m},
        {"RS", 15.0m}, {"RO", 25.0m}, {"RR", 30.0m}, {"SC", 12.0m}, {"SP", 10.0m},
        {"SE", 15.0m}, {"TO", 20.0m}
    };

    return stateShippingCosts.TryGetValue(state.ToUpper(), out var cost) ? cost : 20.0m; // Default cost if state not found
}

app.Run();

public class PaymentData
{
    public decimal Amount { get; set; }
    public string CardNumber { get; set; }
    public string CardholderName { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string CVV { get; set; }
}

public class ShippingData
{
    public int WeightInGrams { get; set; }
    public int HeightInCentimeters { get; set; }
    public int WidthInCentimeters { get; set; }
    public string State { get; set; }
}
