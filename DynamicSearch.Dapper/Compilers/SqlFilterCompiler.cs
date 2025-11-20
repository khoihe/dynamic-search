namespace DynamicSearch.Dapper.Builder;

public class SqlFilterCompiler : IFilterCompiler
{
    protected IDictionary<string, IOperationBuilder> SupportOperations;

    public SqlFilterCompiler(IDictionary<string, IOperationBuilder> supportOperations)
    {
        SupportOperations = supportOperations;
    }

    public (string Query, ExpandoObject Value) Compile(JObject filter, ref int count)
    {
        if (filter.Count != 1)
        {
            var queryFilter = filter.ToObject<QueryFilter>(Defaults.JsonSerializer);

            if (!SupportOperations.ContainsKey(queryFilter.Operation))
                throw new NotSupportedException($"{queryFilter.Operation} is not supported");

            // Validate and safely quote the query key to prevent SQL injection
            queryFilter.QueryKey = SqlIdentifierValidator.QuoteIdentifier(queryFilter.QueryKey);

            var builder = SupportOperations[queryFilter.Operation];
            var result = builder.Build(queryFilter);
            var query = result.Query;
            var objValue = new ExpandoObject();

            for (int i = 0; i < result.Values.Length; i++)
            {
                var valueIndex = $"@{count}";
                query = query.Replace(result.Tokens[i], valueIndex);
                objValue.TryAdd(count.ToString(), result.Values[i]);
                count++;
            }

            return (query, objValue);
        }
        else
        {
            var dictionary = filter.ToObject<IDictionary<string, JArray>>(Defaults.JsonSerializer);
            var listQuery = new List<string>();
            var queryResult = string.Empty;
            var objValue = new ExpandoObject();

            foreach (var item in dictionary)
            {
                // Validate logical operator (only 'and' or 'or' allowed)
                var logicalOperator = item.Key.TrimStart('$').ToLowerInvariant();
                if (logicalOperator != "and" && logicalOperator != "or")
                {
                    throw new ArgumentException(
                        $"Invalid logical operator: '{item.Key}'. Only 'and' or 'or' are allowed.",
                        nameof(filter));
                }

                foreach (var value in item.Value)
                {
                    var result = Compile(value as JObject, ref count);
                    listQuery.Add(result.Query);
                    foreach (var r in result.Value)
                    {
                        objValue.TryAdd(r.Key, r.Value);
                    }
                }
                queryResult = $"( {string.Join($" {logicalOperator} ", listQuery)} )";
            }

            return (queryResult, objValue);
        }
    }
}