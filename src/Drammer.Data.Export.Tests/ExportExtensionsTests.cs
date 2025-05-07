using System.Globalization;

namespace Drammer.Data.Export.Tests;

public sealed class ExportExtensionsTests
{
    [Theory]
    [InlineData("en-US", null, 2, false, null)]
    [InlineData("en-US", 1234567890.123, 2, true, "1,234,567,890.12")]
    [InlineData("en-US", 1234567890.129, 2, true, "1,234,567,890.13")]
    [InlineData("en-US", 1234567890.123, 2, false, "1234567890.12")]
    [InlineData("en-US", 1234567890.123, 0, false, "1234567890")]
    [InlineData("nl-NL", 1234567890.123, 2, true, "1.234.567.890,12")]
    [InlineData("nl-NL", 1234567890.129, 2, true, "1.234.567.890,13")]
    [InlineData("nl-NL", 1234567890.123, 2, false, "1234567890,12")]
    [InlineData("nl-NL", 1234567890.123, 0, false, "1234567890")]
    public void ToStringValue_Decimal(string culture, double? value, int decimals, bool thousandSeparator, string? expected)
    {
        // Arrange
        var cultureInfo = new CultureInfo(culture);
        var d = (decimal?)value;

        // Act
        var result = d.ToStringValue(cultureInfo, decimals, thousandSeparator);

        // Assert
        result.Should().Be(expected);
    }
}