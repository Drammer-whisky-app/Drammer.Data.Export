using ClosedXML.Excel;

namespace Drammer.Data.Export;

/// <summary>
/// The Excel exporter.
/// </summary>
public sealed class ExcelExporter : ExporterBase<byte[], TextHeader>
{
    /// <summary>
    /// The sheet title.
    /// </summary>
    private readonly string? _sheetTitle;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExcelExporter"/> class.
    /// </summary>
    /// <param name="data">
    /// The data.
    /// </param>
    /// <param name="sheetTitle">
    /// The sheet title.
    /// </param>
    public ExcelExporter(DataTable<TextHeader> data, string? sheetTitle = null)
    {
        DataTable = data;
        _sheetTitle = sheetTitle;
    }

    /// <summary>
    /// Export the data to a file.
    /// </summary>
    /// <param name="path">
    /// The path.
    /// </param>
    public void ExportToFile(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(nameof(path));
        }

        var data = Export();
        var f = File.Create(path);
        f.Write(data, 0, data.Length);
        f.Close();
    }

    /// <summary>
    /// Export the data to a file.
    /// </summary>
    /// <param name="path">
    /// The path.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    public async Task ExportToFileAsync(string? path, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(nameof(path));
        }

        var data = Export();
        var f = File.Create(path);
        await f.WriteAsync(data, 0, data.Length, cancellationToken).ConfigureAwait(false);
        f.Close();
    }

    /// <summary>
    /// Export to an Excel file.
    /// </summary>
    /// <returns>
    /// The <see cref="byte"/> array.
    /// </returns>
    public override byte[] Export()
    {
        //// https://github.com/closedxml/closedxml/wiki

        if (DataTable == null)
        {
            throw new InvalidOperationException("DataTable cannot be null");
        }

        using var ms = new MemoryStream();
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(!string.IsNullOrEmpty(_sheetTitle) ? _sheetTitle : "Sheet1");

        // header
        var columnCounter = 1;
        foreach (var h in DataTable.Headers)
        {
            worksheet.Cell(1, columnCounter).SetValue(XLCellValue.FromObject(h.Title));

            columnCounter++;
        }

        // data
        var rowCounter = 2;
        foreach (var d in DataTable.Rows)
        {
            for (var c = 0; c < d.Count; c++)
            {
                var val = d[c];
                worksheet.Cell(rowCounter, c + 1).SetValue(XLCellValue.FromObject(val));
            }

            rowCounter++;
        }

        workbook.SaveAs(ms);
        return ms.ToArray();
    }
}