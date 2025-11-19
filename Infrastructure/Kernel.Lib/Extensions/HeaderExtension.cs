namespace Kernel.Lib.Extension;

public static class HeaderExtension
{
    public static string GetHeaderValue(this IDictionary<string, StringValues> httpRequestHeaders, string key)
    {
        var hasValue = httpRequestHeaders.TryGetValue(key, out var value);
        if (hasValue) return value;
        return null;
    }
}