namespace DynamicSearch.Dapper.Parser;

public class NumericParser : IValueParser<double>
{
    public double Parse(string value)
    {
        return double.Parse(value);
    }
}