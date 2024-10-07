namespace Drammer.Data.Export.Tests;

public sealed class TextHeaderTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Constructor_TitleNotNull()
    {
        // arrange
        var title = _fixture.Create<string>();
        var alignment = _fixture.Create<HeaderTitleAlignment>();

        // act
        var header = new TextHeader(title);

        // assert
        header.Title.Should().Be(title);
        header.Formatter.Should().BeNull();
        header.Alignment.Should().Be(alignment);
    }

    [Fact]
    public void Constructor_TitleNotNull_FormatterNull()
    {
        // arrange
        var title = _fixture.Create<string>();
        Func<object, string>? formatter = null;

        // act
        var header = new TextHeader(title, formatter);

        // assert
        header.Title.Should().Be(title);
        header.Formatter.Should().BeNull();
        header.Alignment.Should().Be(HeaderTitleAlignment.Left);
    }


    [Fact]
    public void Constructor_TitleNotNull_FormatterNotNull()
    {
        // arrange
        var title = _fixture.Create<string>();
        Func<object, string> formatter = x => x.ToString()!;

        // act
        var header = new TextHeader(title, formatter);

        // assert
        header.Title.Should().Be(title);
        header.Formatter.Should().Be(formatter);
        header.Alignment.Should().Be(HeaderTitleAlignment.Left);
    }

    [Fact]
    public void Constructor_TitleNotNull_FormatterNotNull_AlignmentCenter()
    {
        // arrange
        var title = _fixture.Create<string>();
        Func<object, string> formatter = x => x.ToString()!;
        const HeaderTitleAlignment Alignment = HeaderTitleAlignment.Right;

        // act
        var header = new TextHeader(title, formatter, Alignment);

        // assert
        header.Title.Should().Be(title);
        header.Formatter.Should().Be(formatter);
        header.Alignment.Should().Be(Alignment);
    }
}