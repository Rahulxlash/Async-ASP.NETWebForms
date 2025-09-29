using System.Text.Json;
using Mvc4Async.Models;

namespace Mvc4Async.Service;

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
