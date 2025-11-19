namespace Kernel.Lib.Extension;

public static class ObjectExtension
{
    private static List<Type> _supportedTypes = new List<Type>()
    {
        typeof(int),
        typeof(long),
        typeof(double),
        typeof(decimal),
        typeof(int?),
        typeof(long?),
        typeof(double?),
        typeof(decimal?),
        typeof(Guid),
        typeof(Guid?),
        typeof(DateTime),
        typeof(DateTime?)
    };

    private static List<Type> _guidTypes = new List<Type>()
    {
        typeof(Guid),
        typeof(Guid?)
    };

    public static T ConvertTo<T>(this object input)
    {
        try
        {
            if (!_supportedTypes.Any(t => t == typeof(T)))
                throw new NotSupportedException();

            if (input == null || (string)input == string.Empty)
                return default(T);

            var type = typeof(T);
            var nullableType = Nullable.GetUnderlyingType(type);

            // Special case for Guid
            if (_guidTypes.Contains(typeof(T)))
                return (T)(object)new Guid((string)input);

            // Other data types go here
            if (nullableType != null)
                return (T)Convert.ChangeType(input, nullableType);
            else
                return (T)Convert.ChangeType(input, type);
        }
        catch
        {
            return default(T);
        }
    }
}