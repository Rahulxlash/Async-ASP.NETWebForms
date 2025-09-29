using System.Text.Json;
using Mvc4Async.Models;

namespace Mvc4Async.Service;

public class ProductService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ProductService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<Product>> GetProductsAsync(CancellationToken cancelToken = default)
    {
        var uri = Util.GetServiceUri("products", _configuration);
        var response = await _httpClient.GetAsync(uri, cancelToken);
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync(cancelToken);
        return JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<Product>();
    }

    public List<Product> GetProducts()
    {
        var uri = Util.GetServiceUri("products", _configuration);
        var response = _httpClient.GetAsync(uri).Result;
        response.EnsureSuccessStatusCode();
        
        var json = response.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<Product>();
    }
}
