using System.Text.Json;
using Mvc4Async.Models;

namespace Mvc4Async.Service;

public class WidgetService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public WidgetService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<Widget>> GetWidgetsAsync(CancellationToken cancelToken = default)
    {
        var uri = Util.GetServiceUri("widgets", _configuration);
        var response = await _httpClient.GetAsync(uri, cancelToken);
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync(cancelToken);
        return JsonSerializer.Deserialize<List<Widget>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<Widget>();
    }

    public async Task<List<Widget>> GetWidgetsAsync()
    {
        return await GetWidgetsAsync(CancellationToken.None);
    }

    public List<Widget> GetWidgets()
    {
        var uri = Util.GetServiceUri("widgets", _configuration);
        var response = _httpClient.GetAsync(uri).Result;
        response.EnsureSuccessStatusCode();
        
        var json = response.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<List<Widget>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<Widget>();
    }
}
