using System.Globalization;

namespace Drammer.Data.Export.Tests;

public sealed class TextExporterTests
{
    [Fact]
    public void Export_DefaultAlignment_ProducesTextData()
    {
        // arrange
        var data = new DataTable<TextHeader>(new[] { new TextHeader("test1"), new TextHeader("test1") });
        data.AddRow(new DataRow { new string("val1"), new string("val2") });

        var textExporter = new TextExporter(data);

        // act
        var result = textExporter.Export();

        // assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Equal("test1  test1" + Environment.NewLine + "val1   val2 " + Environment.NewLine + "", result);
    }

    [Fact]
    public void Export_AlignRight_ProducesTextData()
    {
        // arrange
        var data = new DataTable<TextHeader>(new[] { new TextHeader("test1", HeaderTitleAlignment.Right), new TextHeader("test1") });
        data.AddRow(new DataRow { new string("val1"), new string("val2") });

        var textExporter = new TextExporter(data);

        var result = textExporter.Export();

        // assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Equal("test1  test1" + Environment.NewLine + " val1  val2 " + Environment.NewLine + "", result);
    }

    [Fact]
    public void Export_WithNumericColumns_ProducesTextData()
    {
        // arrange
        var data = new DataTable<TextHeader>(
            new[] { new TextHeader("test1"), new NumericTextHeader("test2", new CultureInfo("nl-NL"), 2, true) });
        data.AddRow(new DataRow { "val1", 5d });
        data.AddRow(new DataRow { "val2", 5000d });
        data.AddRow(new DataRow { "val3", 5000.12d });

        var textExporter = new TextExporter(data);

        // act
        var result = textExporter.Export();

        // assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Equal("test1     test2" + Environment.NewLine + "val1       5,00" + Environment.NewLine + "val2   5.000,00" + Environment.NewLine + "val3   5.000,12" + Environment.NewLine + "", result);
    }
}