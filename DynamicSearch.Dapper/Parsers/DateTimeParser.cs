namespace DynamicSearch.Dapper.Parser;

public class DateTimeParser : IValueParser<DateTime>
{
    public DateTime Parse(string value)
    {
        return DateTime.ParseExact(value, Defaults.DEFAULT_FULL_DATETIME_FORMAT, CultureInfo.InvariantCulture);
    }
}