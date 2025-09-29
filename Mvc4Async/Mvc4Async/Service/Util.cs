namespace Mvc4Async.Service;

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
