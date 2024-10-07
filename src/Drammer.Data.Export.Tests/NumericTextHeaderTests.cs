namespace Drammer.Data.Export.Tests;

using System.Globalization;
using Xunit;

public class NumericTextHeaderTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Constructor_ValidParameters_SetsProperties()
    {
        // arrange
        var title = _fixture.Create<string>();
        var decimals = _fixture.Create<int>();
        var thousandSeparator = _fixture.Create<bool>();

        // act
        var header = new NumericTextHeader(title, decimals, thousandSeparator);

        // assert
        header.Title.Should().Be(title);
    }

    [Fact]
    public void Constructor_ValidParametersAndCulture_SetsProperties()
    {
        // arrange
        var title = _fixture.Create<string>();
        var decimals = _fixture.Create<int>();
        var thousandSeparator = _fixture.Create<bool>();
        var culture = new CultureInfo("nl-NL");

        // act
        var header = new NumericTextHeader(title, culture, decimals, thousandSeparator);

        // assert
        header.Title.Should().Be(title);
    }

    [Fact]
    public void FormatValue_NullValue_ReturnsEmptyString()
    {
        // arrange
        var header = new NumericTextHeader("Test", 2, true);

        // act
        var result = header.FormatValue(null);

        // assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void FormatValue_ValidNumberWithDecimalsAndThousandSeparator_ReturnsFormattedString()
    {
        // arrange
        var header = new NumericTextHeader("Test", 2, true);

        // act
        var result = header.FormatValue(12345.678);

        // assert
        result.Should().Be("12,345.68");
    }

    [Fact]
    public void FormatValue_ValidNumberWithoutDecimalsAndThousandSeparator_ReturnsFormattedString()
    {
        // arrange
        var header = new NumericTextHeader("Test");

        // act
        var result = header.FormatValue(12345.678);

        // assert
        result.Should().Be("12346");
    }

    [Fact]
    public void FormatValue_ValidNumberWithCustomCulture_ReturnsFormattedString()
    {
        // arrange
        var culture = new CultureInfo("fr-FR");
        var header = new NumericTextHeader("Test", culture, 2, true);

        // act
        var result = header.FormatValue(12345.678);

        // assert
        result.Should().Be($"12{culture.NumberFormat.CurrencyGroupSeparator}345,68");
    }

    [Fact]
    public void FormatValue_ValidNumberWithZeroDecimals_ReturnsFormattedString()
    {
        // arrange
        var header = new NumericTextHeader("Test", 0, true);

        // act
        var result = header.FormatValue(12345.678);

        // assert
        result.Should().Be("12,346");
    }

    [Fact]
    public void FormatValue_ValidNumberWithNegativeDecimals_ReturnsFormattedString()
    {
        // arrange
        var header = new NumericTextHeader("Test", -1, true);

        // act
        var result = header.FormatValue(12345.678);

        // assert
        result.Should().Be("12,346");
    }

    [Fact]
    public void FormatValue_ValidNumberWithNoThousandSeparator_ReturnsFormattedString()
    {
        // arrange
        var header = new NumericTextHeader("Test", 2);

        // act
        var result = header.FormatValue(12345.678);

        // assert
        result.Should().Be("12345.68");
    }
}