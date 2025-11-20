namespace DynamicSearch.Dapper.Builder;

public class NotBetweenOperationBuilder : BetweenOperationBuilder
{
    protected override string Operation => Operations.NOT_BETWEEN;

    public NotBetweenOperationBuilder(IValueArrayParser<double> numbericParser,
                                    IValueArrayParser<DateTime> dateTimeParser)
        : base(numbericParser: numbericParser, dateTimeParser: dateTimeParser)
    {
    }

    protected override (string Query, object[] Values, string[] Tokens) BuildOperationSqlNumber(string fieldName, double[] values)
    {
        if (values.Length != 2) throw new System.Exception("Value should be an array of 2 values");
        string tokenFrom = "@valueFrom", tokenTo = "@valueTo";
        string query = $"{fieldName} < {tokenFrom} or {fieldName} > {tokenTo}";
        return (query, values.Select(x => x as object).ToArray(), new string[] { tokenFrom, tokenTo });
    }

    protected override (string Query, object[] Values, string[] Tokens) BuildOperationSqlDate(string fieldName, DateTime[] values)
    {
        if (values.Length != 2) throw new ArgumentException($"Value should be an array of 2 values");
        string tokenFrom = "@valueFrom", tokenTo = "@valueTo";
        string query = $"({fieldName}::date < {tokenFrom} or {fieldName}::date > {tokenTo})";
        return (query, values.Select(x => x as object).ToArray(), new string[] { tokenFrom, tokenTo });
    }

    protected override (string Query, object[] Values, string[] Tokens) BuildOperationSqlDateTime(string fieldName, DateTime[] values)
    {
        if (values.Length != 2) throw new ArgumentException($"Value should be an array of 2 values");
        string tokenFrom = "@valueFrom", tokenTo = "@valueTo";
        string query = $"({fieldName} < {tokenFrom} or {fieldName} > {tokenTo})";
        return (query, values.Select(x => x as object).ToArray(), new string[] { tokenFrom, tokenTo });
    }
}