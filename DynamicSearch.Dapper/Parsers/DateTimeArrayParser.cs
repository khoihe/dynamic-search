namespace DynamicSearch.Dapper.Parser;

public class DateTimeArrayParser : IValueArrayParser<DateTime>
{
    public DateTime[] Parse(string value)
    {
        var filterArray = value.TrimStart('[').TrimEnd(']').Split(',');
        if (filterArray.Length != 2) throw new System.Exception($"Invalid input of type DateTime");
        var fromDate = DateTime.ParseExact(filterArray[0].Trim(), Defaults.DEFAULT_FULL_DATETIME_FORMAT, CultureInfo.InvariantCulture);
        var toDate = DateTime.ParseExact(filterArray[1].Trim(), Defaults.DEFAULT_FULL_DATETIME_FORMAT, CultureInfo.InvariantCulture);
        return new DateTime[] { fromDate, toDate };
    }
}