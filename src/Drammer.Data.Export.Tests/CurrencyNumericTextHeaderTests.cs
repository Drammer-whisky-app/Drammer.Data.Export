using System.Globalization;

namespace Drammer.Data.Export.Tests;

public sealed class CurrencyNumericTextHeaderTests
{
    [Fact]
    public void FormatValue_ReturnsFormattedValue()
    {
        // arrange
        var header = new CurrencyNumericTextHeader("Title", "$");

        // act
        var result = header.FormatValue(1.2);

        // assert
        result.Should().Be("$ 1.20");
    }

    [Theory]
    [InlineData("€ 1,00", 1d)]
    [InlineData("€ -1,00", -1d)]
    [InlineData("€ 1.000,00", 1000d)]
    [InlineData("€ -1.000.000,00", -1000000d)]
    [InlineData("€ 1,12", 1.123d)]
    [InlineData("€ 1.000,46", 1000.4567d)]
    public void FormatValue_WithCultureInfoAndThousandWithDecimals_ReturnsExpected(string expected, double value)
    {
        // arrange
        var header = new CurrencyNumericTextHeader("my header", new CultureInfo("nl-NL"));

        // act
        var result = header.FormatValue(value);

        result.Should().Be(expected);
    }
}