using System.Globalization;

namespace Drammer.Data.Export;

internal static class ExportExtensions
{
    public static string? ToStringValue(
        this decimal? value,
        CultureInfo cultureInfo,
        int decimals = 2,
        bool thousandSeparator = false)
    {
        if (value == null)
        {
            return null;
        }

        var format = GetFormat(decimals, thousandSeparator);
        return string.Format(cultureInfo, format, value);
    }

    public static string? ToStringValue(this int? value, CultureInfo cultureInfo)
    {
        return value?.ToString(cultureInfo);
    }

    public static string? ToShortDateValue(this DateTimeOffset? value, CultureInfo cultureInfo) =>
        value?.ToShortDateValue(cultureInfo);

    public static string ToShortDateValue(this DateTimeOffset value, CultureInfo cultureInfo) =>
        value.ToString(cultureInfo.DateTimeFormat.ShortDatePattern);

    private static string GetFormat(int decimals, bool thousandSeparator)
    {
        if (thousandSeparator)
        {
            return $"{{0:N{decimals}}}";
        }

        return decimals == 0 ? "{0:0}" : $"{{0:0.{new string('0', decimals)}}}";
    }
}