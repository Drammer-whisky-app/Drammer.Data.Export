namespace Drammer.Data.Export.Tests;

public sealed class DefaultHeaderTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Constructor_WithTitle_TitleIsSet()
    {
        // arrange
        var title = _fixture.Create<string>();

        // act
        var header = new DefaultHeader(title);

        // assert
        header.Title.Should().Be(title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_WithoutTitle_ShouldThrowException(string? title)
    {
        // act
        var act = () => _ = new DefaultHeader(title!);

        // assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WithTitleAndFormatter_TitleIsSet()
    {
        // arrange
        var title = _fixture.Create<string>();
        var formatter = new Func<object, string>(o => o.ToString()!);

        // act
        var header = new DefaultHeader(title, formatter);

        // assert
        header.Title.Should().Be(title);
        header.Formatter.Should().Be(formatter);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_WithoutTitleAndFormatter_ShouldThrowException(string? title)
    {
        // act
        var act = () => _ = new DefaultHeader(title!, null);

        // assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void FormatValue_WithFormatter_FormattedValue()
    {
        // arrange
        var title = _fixture.Create<string>();
        var formatter = new Func<object, string>(o => o.ToString()!);
        var header = new DefaultHeader(title, formatter);
        var value = _fixture.Create<object>();

        // act
        var formattedValue = header.FormatValue(value);

        // assert
        formattedValue.Should().Be(formatter(value));
    }

    [Fact]
    public void FormatValue_WithoutFormatter_FormattedValue()
    {
        // arrange
        var title = _fixture.Create<string>();
        var header = new DefaultHeader(title);
        var value = _fixture.Create<object>();

        // act
        var formattedValue = header.FormatValue(value);

        // assert
        formattedValue.Should().Be(value.ToString());
    }
}