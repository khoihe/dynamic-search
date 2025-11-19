namespace Kernel.Lib.Extension;

public static class StringExtenstion
{
    public static T ConvertTo<T>(this string input)
    {
        return ((object)input).ConvertTo<T>();
    }

    public static string GetString(this string text, params object[] args)
    {
        return string.Format(text, args);
    }
}