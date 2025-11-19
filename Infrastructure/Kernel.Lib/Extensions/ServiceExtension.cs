namespace Kernel.Lib.Extension;

public static class ServiceExtension
{
    public static void AddInternalLogging(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
    }
}