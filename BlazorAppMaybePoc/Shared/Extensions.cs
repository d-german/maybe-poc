namespace BlazorAppMaybePoc.Shared;

public static class Extensions
{
    public static TEnum StringToEnum<TEnum>(this string value, TEnum defaultValue = default) where TEnum : struct
    {
        if (string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        return Enum.TryParse<TEnum>(value, true, out var result) ? result : defaultValue;
    }
}