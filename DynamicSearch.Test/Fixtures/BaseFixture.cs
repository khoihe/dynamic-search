namespace DynamicSearch.Test.Integration;

public abstract class BaseFixture
{
    public const string POSTGRES_IMAGE = "postgres:latest";
    public const string POSTGRES_CONTAINER_NAME = "postgres";
    public const string POSTGRES_DB = "postgres";
    public const string POSTGRES_USER = "postgres";
    public const string POSTGRES_PASSWORD = "Pass123!";
    public const int POSTGRES_PORT = 5432;

    protected INetwork Network { get; }
    protected string ConnectionStringTemplate => "Host={host};Port={port};User Id={user};Password={password};Database={database}";
    private readonly IDictionary<string, string[]> _migrations;

    public BaseFixture(INetwork network, IDictionary<string, string[]> migrations = null!)
    {
        Network = network;
        _migrations = migrations;
    }

    protected async Task RunMigrationAsync()
    {
        if (_migrations is null)
            return;

        foreach (var migration in _migrations)
        {
            var database = migration.Key;
            var connectionString = ConnectionStringTemplate.Replace("{host}", POSTGRES_CONTAINER_NAME)
                                                    .Replace("{port}", POSTGRES_PORT.ToString())
                                                    .Replace("{user}", POSTGRES_USER)
                                                    .Replace("{password}", POSTGRES_PASSWORD)
                                                    .Replace("{database}", database);
            foreach (var migrationPath in migration.Value)
            {
                await RunMigrationWithDirectory(migrationPath, connectionString);
            }
        }
    }

    protected async Task RunMigrationWithDirectory(string directory, string connectionString)
    {
        Console.WriteLine($"Running migration with official image using environment variables. Connection string: {connectionString}");

        // Get the SQL directory path (from bin/Debug/net8.0 to reach solution root)
        var solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
        if (string.IsNullOrWhiteSpace(solutionDirectory))
        {
            throw new InvalidOperationException("Could not find solution directory for SQL files");
        }

        var sqlDirectory = Path.Combine(solutionDirectory, directory);
        if (!Directory.Exists(sqlDirectory))
        {
            throw new InvalidOperationException($"SQL directory not found at: {sqlDirectory}");
        }

        Console.WriteLine($"Using SQL directory: {sqlDirectory}");

        // Use environment variables instead of command line arguments
        var migrationContainer = new ContainerBuilder()
            .WithImage("erikbra/grate:latest")
            .WithNetwork(Network)
            .WithBindMount(sqlDirectory, "/db")
            .WithEnvironment("DATABASE_TYPE", "PostgreSQL")
            .WithEnvironment("CREATE_DATABASE", "true")
            .WithEnvironment("TRANSACTION", "true")
            .WithEnvironment("APP_CONNSTRING", connectionString)
            .WithEnvironment("ENVIRONMENT", "DOCKER")
            .Build();

        try
        {
            Console.WriteLine("Starting migration container with official grate image using environment variables...");
            await migrationContainer.StartAsync();

            // Give the container time to complete
            await Task.Delay(TimeSpan.FromSeconds(1));

            var exitCode = await migrationContainer.GetExitCodeAsync();
            var logs = await migrationContainer.GetLogsAsync();

            Console.WriteLine($"Migration container exit code: {exitCode}");
            Console.WriteLine($"Migration logs:\n{logs}");

            if (exitCode != 0)
            {
                throw new InvalidOperationException($"Migration failed with exit code {exitCode}. Logs: {logs}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during migration with official image: {ex.Message}");
            throw new InvalidOperationException($"Failed to run migration with official grate image: {ex.Message}", ex);
        }
        finally
        {
            await migrationContainer.DisposeAsync();
        }
    }
}