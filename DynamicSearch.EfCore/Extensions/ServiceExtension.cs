namespace DynamicSearch.EfCore.Extension;

public static class ServiceExtension
{
    public static void AddEfCoreDynamicSearch(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IQueryCompiler, QueryCompiler>();
        serviceCollection.AddSingleton<EqualsOperationBuilder>();
        serviceCollection.AddSingleton<NotEqualsOperationBuilder>();
        serviceCollection.AddSingleton<InOperationBuilder>();
        serviceCollection.AddSingleton<NotInOperationBuilder>();
        serviceCollection.AddSingleton<LessThanOperationBuilder>();
        serviceCollection.AddSingleton<LessThanOrEqualsOperationBuilder>();
        serviceCollection.AddSingleton<GreaterThanOperationBuilder>();
        serviceCollection.AddSingleton<GreaterThanOrEqualsOperationBuilder>();
        serviceCollection.AddSingleton<ContainsOperationBuilder>();
        serviceCollection.AddSingleton<NotContainsOperationBuilder>();
        serviceCollection.AddSingleton<AgoOperationBuilder>();
        serviceCollection.AddSingleton<BetweenOperationBuilder>();
        serviceCollection.AddSingleton<NotBetweenOperationBuilder>();
        serviceCollection.AddSingleton<StartsWithOperationBuilder>();
        serviceCollection.AddSingleton<NotStartsWithOperationBuilder>();
        serviceCollection.AddSingleton<EndsWithOperationBuilder>();
        serviceCollection.AddSingleton<NotEndsWithOperationBuilder>();
        serviceCollection.AddSingleton<IValueParser<string>, StringParser>();
        serviceCollection.AddSingleton<IValueParser<double>, NumericParser>();
        serviceCollection.AddSingleton<IValueParser<bool>, BoolParser>();
        serviceCollection.AddSingleton<IValueParser<Guid>, GuidParser>();
        serviceCollection.AddSingleton<IValueArrayParser<Guid>, GuidArrayParser>();
        serviceCollection.AddSingleton<IValueParser<DateTime>, DateTimeParser>();
        serviceCollection.AddSingleton<IValueArrayParser<string>, StringArrayParser>();
        serviceCollection.AddSingleton<IValueArrayParser<double>, NumericArrayParser>();
        serviceCollection.AddSingleton<IValueArrayParser<DateTime>, DateTimeArrayParser>();
        serviceCollection.AddSingleton(GetOperationServices);
    }

    private static IDictionary<string, IOperationBuilder> GetOperationServices(IServiceProvider service)
    {
        return new Dictionary<string, IOperationBuilder>()
        {
            { Operations.EQUALS, service.GetRequiredService<EqualsOperationBuilder>() },
            { Operations.NOT_EQUALS, service.GetRequiredService<NotEqualsOperationBuilder>() },
            { Operations.IN, service.GetRequiredService<InOperationBuilder>() },
            { Operations.NOT_IN, service.GetRequiredService<NotInOperationBuilder>() },
            { Operations.LESS_THAN, service.GetRequiredService<LessThanOperationBuilder>() },
            { Operations.LESS_THAN_OR_EQUALS, service.GetRequiredService<LessThanOrEqualsOperationBuilder>() },
            { Operations.GREATER_THAN, service.GetRequiredService<GreaterThanOperationBuilder>() },
            { Operations.GREATER_THAN_OR_EQUALS, service.GetRequiredService<GreaterThanOrEqualsOperationBuilder>() },
            { Operations.CONTAINS, service.GetRequiredService<ContainsOperationBuilder>() },
            { Operations.NOT_CONTAINS, service.GetRequiredService<NotContainsOperationBuilder>() },
            { Operations.AGO, service.GetRequiredService<AgoOperationBuilder>() },
            { Operations.BETWEEN, service.GetRequiredService<BetweenOperationBuilder>() },
            { Operations.NOT_BETWEEN, service.GetRequiredService<NotBetweenOperationBuilder>() },
            { Operations.STARTS_WITH, service.GetRequiredService<StartsWithOperationBuilder>() },
            { Operations.NOT_STARTS_WITH, service.GetRequiredService<NotStartsWithOperationBuilder>() },
            { Operations.ENDS_WITH, service.GetRequiredService<EndsWithOperationBuilder>() },
            { Operations.NOT_ENDS_WITH, service.GetRequiredService<NotEndsWithOperationBuilder>() }
        };
    }
}