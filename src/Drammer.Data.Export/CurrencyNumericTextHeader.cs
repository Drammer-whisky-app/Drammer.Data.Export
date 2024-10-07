using System.Globalization;

namespace Drammer.Data.Export;

/// <summary>
/// The currency numeric text header.
/// </summary>
public sealed record CurrencyNumericTextHeader : NumericTextHeader
{
    /// <summary>
    /// The currency symbol.
    /// </summary>
    private readonly string _currencySymbol;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrencyNumericTextHeader"/> class.
    /// </summary>
    /// <param name="title">
    /// The title.
    /// </param>
    /// <param name="currencySymbol">
    /// The currency symbol.
    /// </param>
    /// <param name="decimals">
    /// The decimals.
    /// </param>
    /// <param name="thousandSeparator">
    /// The thousand separator.
    /// </param>
    public CurrencyNumericTextHeader(string title, string currencySymbol, int decimals = 2, bool thousandSeparator = true)
        : base(title, decimals, thousandSeparator)
    {
        ArgumentNullException.ThrowIfNull(currencySymbol, nameof(currencySymbol));
        _currencySymbol = currencySymbol;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrencyNumericTextHeader"/> class.
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
    public CurrencyNumericTextHeader(string title, CultureInfo culture, int decimals = 2, bool thousandSeparator = true)
        : base(title, culture, decimals, thousandSeparator)
    {
        ArgumentNullException.ThrowIfNull(culture, nameof(culture));
        _currencySymbol = culture.NumberFormat.CurrencySymbol;
    }

    /// <inheritdoc />
    public override string FormatValue(object? value)
    {
        return _currencySymbol + " " + base.FormatValue(value);
    }
}