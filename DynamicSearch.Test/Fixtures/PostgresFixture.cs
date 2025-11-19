namespace DynamicSearch.Test.Integration;

/// <summary>
/// Fixture for managing PostgreSQL container
/// </summary>
public class PostgresFixture : BaseFixture, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres;

    public string ConnectionString(string database) => ConnectionStringTemplate.Replace("{host}", _postgres.Hostname)
                                                                                .Replace("{port}", _postgres.GetMappedPublicPort(5432).ToString())
                                                                                .Replace("{user}", POSTGRES_USER)
                                                                                .Replace("{password}", POSTGRES_PASSWORD)
                                                                                .Replace("{database}", database);

    public PostgresFixture(INetwork network, IDictionary<string, string[]> migrations = null!)
        : base(network, migrations)
    {
        _postgres = new PostgreSqlBuilder()
            .WithImage(POSTGRES_IMAGE)
            .WithDatabase(POSTGRES_DB)
            .WithUsername(POSTGRES_USER)
            .WithPassword(POSTGRES_PASSWORD)
            .WithPortBinding(POSTGRES_PORT, true)
            .WithNetwork(Network)
            .WithNetworkAliases(POSTGRES_CONTAINER_NAME)
            .Build();
    }

    public async Task InitializeAsync()
    {
        Console.WriteLine($"{nameof(PostgresFixture)} container starting...");
        await _postgres.StartAsync();
        Console.WriteLine($"{nameof(PostgresFixture)} container started");

        Console.WriteLine($"{nameof(PostgresFixture)}:migration starting...");
        await RunMigrationAsync();
        Console.WriteLine($"{nameof(PostgresFixture)}:migration completed...");
    }

    public async Task DisposeAsync()
    {
        Console.WriteLine($"{nameof(PostgresFixture)} container stopping...");
        await _postgres.DisposeAsync();
        Console.WriteLine($"{nameof(PostgresFixture)} container stopped");
    }
}