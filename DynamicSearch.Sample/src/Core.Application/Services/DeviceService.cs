namespace Core.Application.Service;

public class DeviceService : BaseSearchService<Device, string, SearchDevicesCommand, DeviceDto>, IDeviceService
{
    public DeviceService(IServiceProvider serviceProvider)
        : base(serviceProvider, DeviceDto.Create)
    {
    }

    protected override Type GetDbType()
    {
        return typeof(IDeviceRepository);
    }
}