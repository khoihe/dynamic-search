namespace DynamicSearch.Test.Integration;

public class DeviceTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;
    private readonly HttpClient _httpClient;
    private readonly string _host = "http://localhost";
    private readonly string _mediaType = "application/json";

    public DeviceTests(WebApplicationFactory<Startup> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.ConfigureAppConfiguration((hostingContext, configBuilder) =>
            {
                var env = hostingContext.HostingEnvironment;
                configBuilder.AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                configBuilder.AddEnvironmentVariables();
            });
        });
        _httpClient = _factory.CreateClient();
    }

    [Fact]
    public async Task SearchDevices()
    {
        var criteria = new BaseSearchCriteria();
        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(5, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_Equals()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "name" },
            { "queryType", "text" },
            { "operation", "eq" },
            { "queryValue", "Device 1" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(1, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_NotEquals()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "name" },
            { "queryType", "text" },
            { "operation", "neq" },
            { "queryValue", "Device 1" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(4, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_In()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "name" },
            { "queryType", "text" },
            { "operation", "in" },
            { "queryValue", "[Device 1,Device 2]" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(2, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_NotIn()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "name" },
            { "queryType", "text" },
            { "operation", "nin" },
            { "queryValue", "[Device 1,Device 2]" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(3, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_LessThan()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "createdUtc" },
            { "queryType", "datetime" },
            { "operation", "lt" },
            { "queryValue", "2024-01-03T00:00:00:0000" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(2, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_LessThanOrEqualsTo()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "createdUtc" },
            { "queryType", "datetime" },
            { "operation", "lte" },
            { "queryValue", "2024-01-03T00:00:00:0000" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(3, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_GreaterThan()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "createdUtc" },
            { "queryType", "datetime" },
            { "operation", "gt" },
            { "queryValue", "2024-01-03T00:00:00:0000" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(2, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_GreaterThanOrEqualsTo()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "createdUtc" },
            { "queryType", "datetime" },
            { "operation", "gte" },
            { "queryValue", "2024-01-03T00:00:00:0000" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(3, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_Contains_Ref()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "type.name" },
            { "queryType", "text" },
            { "operation", "contains" },
            { "queryValue", "tat" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(1, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_NotContains_Ref()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "type.name" },
            { "queryType", "text" },
            { "operation", "ncontains" },
            { "queryValue", "tat" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(4, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_Between_Ref()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "type.createdUtc" },
            { "queryType", "datetime" },
            { "operation", "between" },
            { "queryValue", "[2024-01-01T00:00:00:0000,2024-01-02T00:00:00:0000]" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(2, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_NotBetween_Ref()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "type.createdUtc" },
            { "queryType", "datetime" },
            { "operation", "nbetween" },
            { "queryValue", "[2024-01-01T00:00:00:0000,2024-01-02T00:00:00:0000]" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(3, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_StartsWith_Ref()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "type.name" },
            { "queryType", "text" },
            { "operation", "sw" },
            { "queryValue", "S" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(1, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_NotStartsWith_Ref()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "queryKey", "type.name" },
            { "queryType", "text" },
            { "operation", "nsw" },
            { "queryValue", "S" }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(4, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_EndsWith_Ref_Multiple()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "and", new JArray
                {
                    new JObject
                    {
                        { "queryKey", "name" },
                        { "queryType", "text" },
                        { "operation", "ew" },
                        { "queryValue", "1" }
                    },
                    new JObject
                    {
                        { "queryKey", "type.name" },
                        { "queryType", "text" },
                        { "operation", "ew" },
                        { "queryValue", "c" }
                    }
                }
            }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(1, response.TotalCount);
    }

    [Fact]
    public async Task SearchDevices_NotEndsWith_Ref_Multiple()
    {
        var criteria = new BaseSearchCriteria();

        criteria.Filter = new JObject
        {
            { "or", new JArray
                {
                    new JObject
                    {
                        { "queryKey", "type.name" },
                        { "queryType", "text" },
                        { "operation", "new" },
                        { "queryValue", "c" }
                    }
                }
            }
        };

        var response = await MakeARequestAsync(criteria);

        Xunit.Assert.Equal(3, response.TotalCount);
    }

    private async Task<BaseSearchResponse<DeviceDto>> MakeARequestAsync(BaseSearchCriteria criteria)
    {
        var payload = new StringContent(JsonConvert.SerializeObject(criteria), Encoding.UTF8, mediaType: _mediaType);
        var response = await _httpClient.PostAsync($"{_host}/dev/devices/search", payload);
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BaseSearchResponse<DeviceDto>>(content);
    }
}