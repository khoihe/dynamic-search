namespace DynamicSearch.EfCore.Service;

internal class DateTimeArrayParser : IValueArrayParser<DateTime>
{
    public DateTime[] Parse(string value)
    {
        var filterArray = value.TrimStart('[').TrimEnd(']').Split(',');

        if (filterArray.Length != 2)
        {
            throw new Exception("Invalid input of type DateTime");
        }

        var fromDateTime = DateTime.ParseExact(filterArray[0].Trim(), Defaults.DEFAULT_FULL_DATETIME_FORMAT, CultureInfo.InvariantCulture);
        var toDateTime = DateTime.ParseExact(filterArray[1].Trim(), Defaults.DEFAULT_FULL_DATETIME_FORMAT, CultureInfo.InvariantCulture);

        var utcFromDateTime = DateTime.SpecifyKind(fromDateTime, DateTimeKind.Utc);
        var utcToDateTime = DateTime.SpecifyKind(toDateTime, DateTimeKind.Utc);

        return new DateTime[] { utcFromDateTime, utcToDateTime };
    }
}