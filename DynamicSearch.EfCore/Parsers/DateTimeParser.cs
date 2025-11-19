namespace DynamicSearch.EfCore.Service;

internal class DateTimeParser : IValueParser<DateTime>
{
    public DateTime Parse(string value)
    {
        var dateTime = DateTime.ParseExact(value, Defaults.DEFAULT_FULL_DATETIME_FORMAT, CultureInfo.InvariantCulture);
        var utcDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        return utcDateTime;
    }
}