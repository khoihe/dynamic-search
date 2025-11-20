namespace DynamicSearch.Dapper.Builder;

public class BetweenOperationBuilder : BaseArrayBuilder
{
    protected override string Operation => Operations.BETWEEN;

    public BetweenOperationBuilder(IValueArrayParser<double> numbericParser,
                                   IValueArrayParser<DateTime> dateTimeParser)
        : base(numbericParser: numbericParser, dateTimeParser: dateTimeParser)
    {
        SupportOperations.Clear();
        SupportOperations.Add(QueryType.DATE, BuildDate);
        SupportOperations.Add(QueryType.DATETIME, BuildDateTime);
    }


    protected override (string Query, object[] Values, string[] Tokens) BuildOperationSqlDate(string fieldName, DateTime[] values)
    {
        if (values.Length != 2) throw new ArgumentException($"Value should be an array of 2 values");
        string tokenFrom = "@valueFrom", tokenTo = "@valueTo";
        string query = $"( {tokenFrom} <= {fieldName}::date and {fieldName}::date <= {tokenTo} )";
        return (query, values.Select(x => x as object).ToArray(), new string[] { tokenFrom, tokenTo });
    }

    protected override (string Query, object[] Values, string[] Tokens) BuildOperationSqlDateTime(string fieldName, DateTime[] values)
    {
        if (values.Length != 2) throw new ArgumentException($"Value should be an array of 2 values");
        string tokenFrom = "@valueFrom", tokenTo = "@valueTo";
        string query = $"( {tokenFrom} <= {fieldName} and {fieldName} <= {tokenTo} )";
        return (query, values.Select(x => x as object).ToArray(), new string[] { tokenFrom, tokenTo });
    }
}