namespace DynamicSearch.Test.Integration;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly CompositeFixture _compositeFixture;

    public TestWebApplicationFactory(CompositeFixture compositeFixture)
    {
        _compositeFixture = compositeFixture;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Local");

        // Still read from appsettings.json but this config is to override specific settings for tests
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = _compositeFixture.DatabaseFixture.ConnectionString(database: "core")
            };

            // This adds the in-memory configuration
            configBuilder.AddInMemoryCollection(settings);
        });
    }
}