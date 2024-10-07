namespace Drammer.Data.Export.Tests;

public sealed class DataTableTests
{
    [Fact]
    public void Constructor_WithEnumerable_PropertySet()
    {
        // arrange
        var textHeaders = new List<TextHeader> {new("Title1"), new("Title2")};

        // act
        var result = new DataTable<TextHeader>(textHeaders);

        // assert
        result.Headers.Should().BeEquivalentTo(textHeaders);
    }

    [Fact]
    public void Constructor_WithoutEnumerable_PropertySet()
    {
        // act
        var result = new DataTable<TextHeader>(null);

        // assert
        result.Headers.Should().BeEmpty();
    }

    [Fact]
    public void AddRow_WithValidRow_RowAdded()
    {
        // arrange
        var textHeaders = new List<TextHeader> {new("Title1"), new("Title2")};
        var dataTable = new DataTable<TextHeader>(textHeaders);

        // act
        dataTable.AddRow(["Value1", "Value2"]);

        // assert
        dataTable.Rows.Should().NotBeEmpty();
    }

    [Fact]
    public void AddRow_WithInvalidRow_ThrowException()
    {
        // arrange
        var textHeaders = new List<TextHeader> {new("Title1"), new("Title2")};
        var dataTable = new DataTable<TextHeader>(textHeaders);

        // act
        var action = () => dataTable.AddRow(["Value1"]);

        // assert
        action.Should().Throw<ArgumentException>();
    }
}