using System.Globalization;
using System.Text;

namespace Drammer.Data.Export;

/// <summary>
/// The numeric text header.
/// </summary>
public record NumericTextHeader : TextHeader
{
    /// <summary>
    /// The decimals.
    /// </summary>
    private readonly int _decimals;

    /// <summary>
    /// The thousand separator.
    /// </summary>
    private readonly bool _thousandSeparator;

    /// <summary>
    /// The default culture.
    /// </summary>
    private readonly CultureInfo _defaultCulture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Initializes a new instance of the <see cref="NumericTextHeader"/> class.
    /// </summary>
    /// <param name="title">
    /// The title.
    /// </param>
    /// <param name="decimals">
    /// The decimals.
    /// </param>
    /// <param name="thousandSeparator">
    /// The thousand separator.
    /// </param>
    public NumericTextHeader(string title, int decimals = 0, bool thousandSeparator = false)
        : base(title, HeaderTitleAlignment.Right)
    {
        _decimals = decimals;
        _thousandSeparator = thousandSeparator;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NumericTextHeader"/> class.
    /// </summary>
    /// <param name="title">
    /// The title.
    /// </param>
    /// <param name="culture">
    /// The culture.
    /// </param>
    /// <param name="decimals">
    /// The decimals.
    /// </param>
    /// <param name="thousandSeparator">
    /// The thousand separator.
    /// </param>
    public NumericTextHeader(string title, CultureInfo culture, int decimals = 0, bool thousandSeparator = false)
        : base(title, HeaderTitleAlignment.Right)
    {
        _decimals = decimals;
        _thousandSeparator = thousandSeparator;
        _defaultCulture = culture ?? throw new ArgumentNullException(nameof(culture));
    }

    /// <inheritdoc />
    public override string FormatValue(object? value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        var format = GetFormat();
        return string.Format(_defaultCulture, format, value);
    }

    /// <summary>
    /// Gets the format string.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    private string GetFormat()
    {
        var sb = new StringBuilder();

        sb.Append("0:");

        if (_thousandSeparator)
        {
            sb.Append("#,");
        }

        if (_decimals > 0)
        {
            sb.AppendFormat(
                "0.{0}",
                string.Join(string.Empty, Enumerable.Range(0, _decimals).Select(_ => "0")));
        }
        else
        {
            sb.Append("0");
        }

        return $"{{{sb}}}";
    }
}