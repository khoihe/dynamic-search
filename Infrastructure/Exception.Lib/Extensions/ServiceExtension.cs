namespace Exception.Lib.Extension;

public static class ServiceExtension
{
    public static void AddExceptionHandling(this MvcOptions options)
    {
        options.Filters.Add(typeof(GlobalExceptionFilter));
    }
}