using Microsoft.Extensions.Configuration;

namespace WebAPIpgw.Utility;

public class Util
{
    private static readonly Random random = new();
    private static IConfiguration? _configuration;

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static int GetDelay()
    {
        return random.Next(Configuration.GetDelayMin(),
                             Configuration.GetDelayMax());
    }
}

public static class Configuration
{
    private static int _delayMin = int.MaxValue;
    private static int _delayMax = int.MaxValue;
    private static IConfiguration? _configuration;

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static int GetKeyValInt(string key)
    {
        if (_configuration == null)
            return 0;

        if (int.TryParse(_configuration[key], out int result))
            return result;
        else
            return 0;
    }

    public static int GetDelayMin()
    {
        if (_delayMin == int.MaxValue)
            _delayMin = GetKeyValInt("DelayMin");
        return _delayMin;
    }

    public static int GetDelayMax()
    {
        if (_delayMax == int.MaxValue)
            _delayMax = GetKeyValInt("DelayMax");
        return _delayMax;
    }

    public static string? GetKeyVal(string key)
    {
        return _configuration?[key];
    }
}
