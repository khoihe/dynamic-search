namespace Core.Application.Extension;

public static class ObjectExtension
{
    public static dynamic ToExpandoObject(this object value)
    {
        IDictionary<string, object> dapperRowProperties = value as IDictionary<string, object>;
        IDictionary<string, object> expando = new ExpandoObject();

        foreach (KeyValuePair<string, object> property in dapperRowProperties)
        {
            var valueProperty = property.Value;
            if (valueProperty == null)
            {
                expando.Add(property.Key, valueProperty);
                continue;
            }
            if (valueProperty.GetType() == typeof(DateTime))
            {
                var datetimeValue = Convert.ToDateTime(valueProperty).ToString(Defaults.DEFAULT_FULL_DATETIME_FORMAT);
                expando.Add(property.Key, datetimeValue);
            }
            else
            {
                expando.Add(property.Key, valueProperty);
            }
        }

        return expando as ExpandoObject;
    }
}