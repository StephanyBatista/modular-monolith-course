using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using EGeek.Catalog.Contract;
using EGeek.Purchase.Checkout;
using EGeek.Purchase.ShippingCost;
using Microsoft.Extensions.Configuration;

namespace EGeek.Purchase.Infra.ShippingCost;

internal class ShippingCostApi : IShippingCost
{
    private readonly HttpClient _client;
    private readonly string _url;

    public ShippingCostApi(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _client = httpClientFactory.CreateClient();
        _url = configuration.GetValue<string>("Purchase:ShippingUrl")!;
    }
    
    public async Task<decimal> GetCost(List<GetProductResponse> products, string zipCode)
    {
        var bodyItems = new List<BodyItem>();

        foreach (var item in products)
        {
            var bodyItem = new BodyItem(item.WeightInGrams, item.HeightInCentimeters, item.WidthInCentimeters);
            bodyItems.Add(bodyItem);
        }
        var body = new BodyShippingCost(zipCode, bodyItems);
        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync($"{_url}/calculate-shipping", content);

        response.EnsureSuccessStatusCode();
        var responseCost = await response.Content.ReadFromJsonAsync<ResponseShippingCost>();
        
        return responseCost.Cost;
    }
}