using System.Text.Json;
using WebAppAsync.Models;

namespace WebAppAsync.Service;

public class GizmoService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GizmoService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<Gizmo>> GetGizmosAsync(CancellationToken cancelToken = default)
    {
        var uri = Util.GetServiceUri("Gizmos", _configuration);
        var response = await _httpClient.GetAsync(uri, cancelToken);
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync(cancelToken);
        return JsonSerializer.Deserialize<List<Gizmo>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<Gizmo>();
    }

    public async Task<List<Gizmo>> GetGizmosAsync()
    {
        return await GetGizmosAsync(CancellationToken.None);
    }

    public List<Gizmo> GetGizmos()
    {
        var uri = Util.GetServiceUri("Gizmos", _configuration);
        var response = _httpClient.GetAsync(uri).Result;
        response.EnsureSuccessStatusCode();
        
        var json = response.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<List<Gizmo>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<Gizmo>();
    }
}

public static class Util
{
    public static string GetRootUri(IConfiguration configuration)
    {
        var uri = configuration["WidgetServiceURI"];
        return string.IsNullOrEmpty(uri) ? "http://localhost:7734/" : uri;
    }

    public static string GetServiceUri(string srv, IConfiguration configuration)
    {
        return GetRootUri(configuration) + "api/" + srv;
    }
}
