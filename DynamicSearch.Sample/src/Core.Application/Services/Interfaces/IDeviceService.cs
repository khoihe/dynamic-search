namespace Core.Application.Service.Interface;

public interface IDeviceService : ISearchService<Device, string, SearchDevicesCommand, DeviceDto>
{
}