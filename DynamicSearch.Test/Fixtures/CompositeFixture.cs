namespace DynamicSearch.Test.Integration;

/// <summary>
/// Base composite fixture that manages core infrastructure containers (Database, Redis, RabbitMQ...)
/// </summary>
public class CompositeFixture : IAsyncLifetime
{
    protected readonly INetwork _network = new NetworkBuilder().Build();

    public PostgresFixture DatabaseFixture { get; protected set; }

    public CompositeFixture()
    {
        DatabaseFixture = new PostgresFixture(_network, migrations: new Dictionary<string, string[]>
        {
            ["core"] = new[] { "./sql" }
        });
    }

    public virtual async Task InitializeAsync()
    {
        await _network.CreateAsync();

        await Task.WhenAll(
            DatabaseFixture.InitializeAsync()
        );
    }

    public virtual async Task DisposeAsync()
    {
        await Task.WhenAll(
            DatabaseFixture.DisposeAsync()
        );

        await _network.DisposeAsync();
    }
}