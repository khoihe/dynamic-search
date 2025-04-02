namespace Core.Application.Command;

public class SearchDevicesCommand : BaseSearchCriteria, IRequest<BaseSearchResponse<DeviceDto>>
{
    public SearchDevicesCommand()
    {
        PageSize = 20;
        PageIndex = 0;
    }
}