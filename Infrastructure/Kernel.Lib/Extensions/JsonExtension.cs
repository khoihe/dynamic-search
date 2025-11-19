namespace Kernel.Lib.Extension;

/// <summary>
/// Serialization/Deserialization using UTF-8 encoding takes less space and reduce the overhead in case the data being sent over the network
/// </summary> <summary>
public static class JsonExtension
{
    /// <summary>
    /// Uniform rules to control over the json object
    /// </summary>    
    private static JsonSerializer _js = new JsonSerializer
    {
        NullValueHandling = NullValueHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Error,
        DateFormatString = Defaults.DEFAULT_FULL_DATETIME_FORMAT,
        DateParseHandling = DateParseHandling.None,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    /// <summary>
    /// Uniform rules to control over the json object
    /// </summary>
    private static JsonSerializerSettings _jss = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Error,
        DateFormatString = Defaults.DEFAULT_FULL_DATETIME_FORMAT,
        DateParseHandling = DateParseHandling.None,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public static JsonSerializer JsonSerializer => _js;
    public static JsonSerializerSettings JsonSerializerSetting => _jss;

    /// <summary>
    /// Serialize an object to bytes
    /// </summary>
    public static byte[] Serialize(this object input)
    {
        byte[] output = default;

        using (var ms = new MemoryStream())
        using (var sw = new StreamWriter(ms))
        using (var jtw = new JsonTextWriter(sw))
        {
            JsonSerializer.Serialize(jtw, input);
            jtw.Flush();
            output = ms.ToArray();
        }

        return output;
    }

    /// <summary>
    /// Deserialize bytes to an object
    /// </summary>
    public static T Deserialize<T>(this byte[] input)
    {
        T output = default;

        using (var ms = new MemoryStream(input))
        using (var sr = new StreamReader(ms))
        using (var jtr = new JsonTextReader(sr))
        {
            output = JsonSerializer.Deserialize<T>(jtr);
        }

        return output;
    }

    /// <summary>
    /// Convert an object to json string
    /// </summary>
    public static string ToJson(this object input)
    {
        var output = Serialize(input);
        return Encoding.UTF8.GetString(output);
    }

    /// <summary>
    /// Convert json string to an object
    /// </summary>
    public static T ToObject<T>(this string input)
    {
        var output = Encoding.UTF8.GetBytes(input);
        return Deserialize<T>(output);
    }

    /// <summary>
    /// Convert an object to byte an array
    /// </summary>
    public static byte[] ToByteArray<T>(T input)
    {
        if (input == null)
            return null;
        return Serialize(input);
    }

    /// <summary>
    /// Convert an byte array to an object
    /// </summary>
    public static (bool Success, T Result) ToObjectWithResult<T>(this byte[] input)
    {
        if (input == null)
            return (false, default(T));
        return (true, Deserialize<T>(input));
    }
}