namespace Drammer.Data.Export.Tests;

public sealed class CsvExporterTests
{
    [Fact]
    public void Export_ProducesCsvData()
    {
        // arrange
        var data = new DataTable<TextHeader>(new[] { new TextHeader("test1"), new TextHeader("test1") });
        data.AddRow(new DataRow { new string("val1"), new string("val2") });

        var csvExporter = new CsvExporter(data);

        // act
        var result = csvExporter.Export();

        // assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("test1;test1" + Environment.NewLine + "val1;val2" + Environment.NewLine + "");
    }

    [Fact]
    public void Export_WithNullValues_ProducesCsvData()
    {
        // arrange
        var data = new DataTable<TextHeader>(new[] { new TextHeader("test1"), new TextHeader("test1") });
        data.AddRow(new DataRow { null, new string("val2") });

        var csvExporter = new CsvExporter(data);

        // act
        var result = csvExporter.Export();

        // assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("test1;test1" + Environment.NewLine + ";val2" + Environment.NewLine + "");
    }

    [Fact]
    public void Export_NoCsvInjection()
    {
        // arrange
        var data = new DataTable<TextHeader>(new[] { new TextHeader("=test1"), new TextHeader("test1") });
        data.AddRow(new DataRow { new string("+val1"), new string("val2") });

        var csvExporter = new CsvExporter(data);

        // act
        var result = csvExporter.Export();

        // assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("'=test1;test1" + Environment.NewLine + "'+val1;val2" + Environment.NewLine + "");
    }

    [Fact]
    public void ExportToByteArray()
    {
        // arrange
        var data = new DataTable<TextHeader>(new[] { new TextHeader("test1"), new TextHeader("test1") });
        data.AddRow(new DataRow { new string("val1"), new string("val2") });

        var csvExporter = new CsvExporter(data);

        // act
        var result = csvExporter.ExportToFile();

        // assert
        result.Length.Should().BeGreaterThan(0);

        var resultString = System.Text.Encoding.Default.GetString(result);
        resultString.Should().NotBeNullOrEmpty();
        resultString.Should().Be("test1;test1" + Environment.NewLine + "val1;val2" + Environment.NewLine + "");
    }

    [Fact]
    public async Task ExportToByteArrayAsync()
    {
        // arrange
        var data = new DataTable<TextHeader>(new[] { new TextHeader("test1"), new TextHeader("test1") });
        data.AddRow(new DataRow { new string("val1"), new string("val2") });

        var csvExporter = new CsvExporter(data);

        // act
        var result = await csvExporter.ExportToFileAsync();

        // assert
        result.Length.Should().BeGreaterThan(0);

        var resultString = System.Text.Encoding.Default.GetString(result);
        resultString.Should().NotBeNullOrEmpty();
        resultString.Should().Be("test1;test1" + Environment.NewLine + "val1;val2" + Environment.NewLine + "");
    }
}