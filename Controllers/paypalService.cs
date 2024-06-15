using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using grupo_rojo.Models;

public class payPalService : IPayPalService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public payPalService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var clientId = _configuration["paypalOptions:ClientId"];
        var clientSecret = _configuration["paypalOptions:ClientSecret"];
        var authToken = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

        var response = await _httpClient.PostAsync("https://api.sandbox.paypal.com/v1/oauth2/token", new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded"));
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(json);
        return result.GetProperty("access_token").GetString();
    }

    public async Task<string> CreatePaymentAsync(decimal total, string currency, string returnUrl, string cancelUrl)
    {
        var accessToken = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var payment = new
        {
            intent = "sale",
            payer = new { payment_method = "paypal" },
            transactions = new[]
            {
                new
                {
                    amount = new { total = total.ToString("F"), currency },
                    description = "Compra en mi tienda"
                }
            },
            redirect_urls = new { return_url = returnUrl, cancel_url = cancelUrl }
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(payment), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://api.sandbox.paypal.com/v1/payments/payment", jsonContent);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(jsonResponse);

        return result.GetProperty("links").EnumerateArray().First(x => x.GetProperty("rel").GetString() == "approval_url").GetProperty("href").GetString();
    }
}