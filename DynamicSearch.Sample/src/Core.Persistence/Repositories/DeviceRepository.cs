namespace Core.Persistence.Repository
{
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

        protected override void Update(Device requestObject, Device targetObject)
        {
            targetObject.Name = requestObject.Name;
            targetObject.TypeId = requestObject.TypeId;
            targetObject.UpdatedUtc = DateTime.UtcNow;
        }
    }
}