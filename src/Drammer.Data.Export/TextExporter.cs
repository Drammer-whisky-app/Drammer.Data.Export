using System.Text;

namespace Drammer.Data.Export;

/// <summary>
/// The text exporter.
/// </summary>
public sealed class TextExporter : ExporterBase<string, TextHeader>
{
    /// <summary>
    /// The separator.
    /// </summary>
    private readonly string _separator;

    /// <summary>
    /// The file header.
    /// </summary>
    private readonly string? _fileHeader;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextExporter"/> class.
    /// </summary>
    /// <param name="data">
    /// The data.
    /// </param>
    /// <param name="separator">
    /// The separator.
    /// </param>
    /// <param name="fileHeaderGenerator">
    /// The file header generator function.
    /// </param>
    public TextExporter(DataTable<TextHeader> data, string? separator = "  ", Func<string>? fileHeaderGenerator = null)
    {
        DataTable = data;
        _separator = separator ?? string.Empty;

        if (fileHeaderGenerator != null)
        {
            _fileHeader = fileHeaderGenerator.Invoke();
        }
    }

    /// <summary>
    /// Exports the data.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public override string Export()
    {
        var maxLengths = GetMaxLength();
        var sb = new StringBuilder();

        // add header
        if (!string.IsNullOrEmpty(_fileHeader))
        {
            sb.Append(_fileHeader);
            sb.AppendLine();
        }

        // export header
        for (var i = 0; i < DataTable!.Headers.Count; i++)
        {
            if (i > 0)
            {
                sb.Append(_separator);
            }

            sb.Append(
                DataTable.Headers[i].Alignment == HeaderTitleAlignment.Right
                    ? DataTable.Headers[i].Title.PadLeft(maxLengths[i])
                    : DataTable.Headers[i].Title.PadRight(maxLengths[i]));
        }

        sb.AppendLine();

        // export data
        foreach (var row in DataTable.Rows)
        {
            var columns = ApplyFormat(row);
            for (var y = 0; y < columns.Count; y++)
            {
                if (y > 0)
                {
                    sb.Append(_separator);
                }

                var value = !string.IsNullOrEmpty(columns[y]) ? columns[y] : string.Empty;

                sb.Append(
                    DataTable.Headers[y].Alignment == HeaderTitleAlignment.Right ? value.PadLeft(maxLengths[y]) : value.PadRight(maxLengths[y]));
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    /// <summary>
    /// Get the max length of each column.
    /// </summary>
    /// <returns>
    /// The <see cref="int"/> array.
    /// </returns>
    private int[] GetMaxLength()
    {
        var res = new int[DataTable!.Headers.Count];
        for (var i = 0; i < res.Length; i++)
        {
            res[i] = GetMaxLength(i);
        }

        return res;
    }

    /// <summary>
    /// Returns the max length of a column comparing all values.
    /// </summary>
    /// <param name="columnIndex">
    /// The column Index.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    private int GetMaxLength(int columnIndex)
    {
        var length = 0;

        foreach (var row in DataTable!.Rows)
        {
            var columns = ApplyFormat(row);
            var column = columns[columnIndex];
            if (!string.IsNullOrEmpty(column) && column.Length > length)
            {
                length = column.Length;
            }
        }

        if (DataTable.Headers[columnIndex].Title.Length > length)
        {
            length = DataTable.Headers[columnIndex].Title.Length;
        }

        return length;
    }
}