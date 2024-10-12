using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using EGeek.Purchase.Checkout;
using Microsoft.Extensions.Configuration;

namespace EGeek.Purchase.Infra.Payment;

internal class PaymentApi : IPayment
{
    private readonly HttpClient _client;
    private readonly string _url;

    public PaymentApi(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _client = httpClientFactory.CreateClient();
        _url = configuration.GetValue<string>("Purchase:ShippingUrl")!;
    }
    
    public async Task<(bool, string)> Process(
        decimal amount,
        string cardNumber,
        string cardHolderName,
        DateTime expirationDate,
        string cvv
    )
    {
        var body = new
        {
            amount, cardNumber, cardHolderName, expirationDate, cvv
        };
        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync($"{_url}/process-payment", content);
        response.EnsureSuccessStatusCode();
        var responsePayment = await response.Content.ReadFromJsonAsync<ResponsePayment>();
        
        return (responsePayment.Approved, responsePayment.Message);
    }
}