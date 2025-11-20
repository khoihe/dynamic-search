namespace DynamicSearch.EfCore.Service;

internal class NumericParser : IValueParser<double>
{
    public double Parse(string value)
    {
        return double.Parse(value);
    }
}