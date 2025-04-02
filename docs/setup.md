# Setup ([detailed instruction](../DynamicSearch.Sample))
### Register DI
```C#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEfCoreDynamicSearch();
    }
}
```

### Entity inherits from `IEntity`
```C#
public class Device : IEntity<string>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string TypeId { get; set; }
    public DeviceType Type { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public bool Deleted { set; get; }
}
```

### Repository interface inherits from `IRepository`
```C#
public interface IDeviceRepository : IRepository<Device, string>
{
}
```

### Repository inherits from `GenericRepository`
```C#
public class DeviceRepository : GenericRepository<Device, string>, IDeviceRepository
{
    private CoreDbContext _context;
    private readonly IConfiguration _configuration;

    public DeviceRepository(CoreDbContext context, IConfiguration configuration)
        : base(context)
    {
        _context = context;
        _configuration = configuration;
    }

    public override IQueryable<Device> AsQueryable() => _context.Devices.Include(x => x.Type);
}
```

### Search command inherits from `BaseSearchCriteria`
```C#
public class SearchDevicesCommand : BaseSearchCriteria
{
    public SearchDevicesCommand()
    {
        PageSize = 20;
        PageIndex = 0;
    }
}
```

### Implement projection for `DTO`
```C#
public class DeviceDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public DeviceTypeDto Type { get; set; }

    private static Func<Device, DeviceDto> Converter = Projection.Compile();

    public static Expression<Func<Device, DeviceDto>> Projection
    {
        get
        {
            return entity => new DeviceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedUtc = entity.CreatedUtc,
                UpdatedUtc = entity.UpdatedUtc,
                Type = DeviceTypeDto.Create(entity.Type)
            };
        }
    }

    public static DeviceDto Create(Device entity)
    {
        if (entity == null)
            return null;
        return Converter(entity);
    }
}
```

### Service interface inherits from `ISearchService`
```C#
public interface IDeviceService : ISearchService<Device, string, SearchDevicesCommand, DeviceDto>
{
}
```

### Service inherits from BaseSearchService
```C#
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
```

### Inject the service and user the search function anywhere
```C#
public class SearchDevicesRequestHandler : IRequestHandler<SearchDevicesCommand, BaseSearchResponse<DeviceDto>>
{
    private readonly IDeviceService _service;

    public SearchDevicesRequestHandler(IDeviceService service)
    {
        _service = service;
    }

    public Task<BaseSearchResponse<DeviceDto>> Handle(SearchDevicesCommand request, CancellationToken cancellationToken)
    {
        return _service.SearchAsync(request);
    }
}
```