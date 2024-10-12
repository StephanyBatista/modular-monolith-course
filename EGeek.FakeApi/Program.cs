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
    var random = new Random();
    var totalCost = 0;
    foreach (var item in request.Products)
    {
        totalCost += random.Next(10, 110);
    }
    
    return Results.Json(new {cost = totalCost});
});


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
    public string ZipCode { get; set; }
    public List<ProductData> Products { get; set; }
}

public class ProductData
{
    public int WeightInGrams { get; set; }
    public int HeightInCentimeters { get; set; }
    public int WidthInCentimeters { get; set; }
}
