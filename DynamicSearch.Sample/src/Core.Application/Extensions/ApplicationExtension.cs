namespace Core.Application.Extension;

public static class ApplicationExtension
{
    public const string SERVICE_NAME = "core-service";

    public static void AddApplicationServices(this IServiceCollection serviceCollection)
    {
        // Lib services
        serviceCollection.AddEfCoreDynamicSearch();
        serviceCollection.AddMediatR(typeof(ApplicationExtension).GetTypeInfo().Assembly);

        // Internal services
        serviceCollection.AddScoped<IDeviceService, DeviceService>();
    }
}