namespace Kernel.Lib.Constant;

public static class Defaults
{
    private static JsonSerializer _js = new JsonSerializer
    {
        NullValueHandling = NullValueHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Error,
        DateFormatString = DEFAULT_FULL_DATETIME_FORMAT,
        DateParseHandling = DateParseHandling.None,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    private static JsonSerializerSettings _jss = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Error,
        DateFormatString = DEFAULT_FULL_DATETIME_FORMAT,
        DateParseHandling = DateParseHandling.None,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public static string DEFAULT_FULL_DATETIME_FORMAT = "yyyy-MM-ddTHH:mm:ss:ffff";
    public static string DEFAULT_SHORT_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
    public static string DEFAULT_ALIAS_DATETIME_FORMAT = "yyyyMMddHHmmss";
    public static JsonSerializer JsonSerializer => _js;
    public static JsonSerializerSettings JsonSerializerSetting => _jss;
}