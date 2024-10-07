using ClosedXML.Excel;

namespace Drammer.Data.Export.Tests;

public sealed class ExcelExporterTests
{
    [Fact]
    public void Export_ProducesWorkbookDocument()
    {
        // arrange
        var data = new DataTable<TextHeader>(new[] { new TextHeader("test1"), new TextHeader("test2") });
        data.AddRow(new DataRow { new string("val1"), new string("val2") });
        data.AddRow(new DataRow { 5, 10 });

        var excelExporter = new ExcelExporter(data);

        // act
        var workbookData = excelExporter.Export();

        // assert
        using var workbook = new XLWorkbook(new MemoryStream(workbookData));
        var firstSheet = workbook.Worksheets.First();
        Assert.Equal("test2", firstSheet.Cell(1, 2).Value);
        Assert.Equal("val1", firstSheet.Cell(2, 1).Value);
        Assert.Equal(10d, firstSheet.Cell(3, 2).Value); // value is converted to double

        Assert.Equal(XLDataType.Text, firstSheet.Cell(2, 1).DataType);
        Assert.Equal(XLDataType.Number, firstSheet.Cell(3, 1).DataType);
    }
}