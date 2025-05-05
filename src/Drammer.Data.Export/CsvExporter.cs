using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Drammer.Data.Export;

/// <summary>
/// The CSV exporter.
/// </summary>
public sealed class CsvExporter : ExporterBase<string, TextHeader>, IExportableAsync
{
    /// <summary>
    /// The delimiter.
    /// </summary>
    private readonly string _delimiter;

    /// <summary>
    /// Initializes a new instance of the <see cref="CsvExporter"/> class.
    /// </summary>
    /// <param name="data">
    /// The data.
    /// </param>
    /// <param name="delimiter">
    /// The delimiter.
    /// </param>
    public CsvExporter(DataTable<TextHeader> data, string? delimiter = ";")
    {
        DataTable = data;
        _delimiter = delimiter ?? string.Empty;
    }

    /// <summary>
    /// The export.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public override string Export()
    {
        if (DataTable == null)
        {
            throw new ArgumentException("DataTable cannot be null.");
        }

        var sb = new StringBuilder();

        // export header
        sb.AppendLine(string.Join(_delimiter, DataTable.Headers.Select(x => ApplyRfc4180(x.Title))));

        // export data
        foreach (var row in DataTable.Rows)
        {
            sb.AppendLine(string.Join(_delimiter, ApplyFormat(row)));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Export the data to a byte array.
    /// </summary>
    /// <param name="encoding">
    /// The encoding.
    /// </param>
    /// <returns>
    /// The <see cref="byte"/>.
    /// </returns>
    public byte[] ExportToFile(Encoding? encoding = null)
    {
        var data = Export();
        using var ms = new MemoryStream();
        var writer = new StreamWriter(ms, encoding ?? Encoding.Default);
        writer.Write(data);
        writer.Flush();
        ms.Position = 0;
        return ms.ToArray();
    }

    /// <summary>
    /// Export the data to a byte array async
    /// </summary>
    /// <param name="encoding">
    /// The encoding.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    public async Task<byte[]> ExportToFileAsync(Encoding? encoding = null, CancellationToken cancellationToken = default)
    {
        var data = Export();
        using var ms = new MemoryStream();
        var writer = new StreamWriter(ms, encoding ?? Encoding.Default);
        await writer.WriteAsync(data).ConfigureAwait(false);
        await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
        ms.Position = 0;
        return ms.ToArray();
    }

    /// <summary>
    /// The apply format.
    /// </summary>
    /// <param name="dataRow">
    /// The data row.
    /// </param>
    /// <returns>
    /// The <see cref="List{T}"/>.
    /// </returns>
    protected override List<string> ApplyFormat(List<object?> dataRow)
    {
        if (DataTable == null)
        {
            throw new ArgumentException("DataTable cannot be null.");
        }

        if (dataRow.Count != DataTable.Headers.Count)
        {
            throw new ArgumentException($"Items in dataRow ({dataRow.Count}) and header ({DataTable.Headers.Count}) are not equal.");
        }

        return dataRow.Select((t, i) => ApplyRfc4180(DataTable.Headers[i].FormatValue(t))).ToList();
    }

    /// <summary>
    /// The apply RFC 4180.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    [return: NotNullIfNotNull(nameof(value))]
    private static string? ApplyRfc4180(string? value)
    {
        //// see: https://tools.ietf.org/html/rfc4180

        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        var result = value;

        // If double-quotes are used to enclose fields, then a double-quote appearing inside a
        // field must be escaped by preceding it with another double quote
        if (value.IndexOf('"') >= 0)
        {
            result = value.Replace("\"", "\"\"");
        }

        // Fields containing line breaks (CRLF), double quotes, and commas should be enclosed in double-quotes.
        const string SpecialChars = ",\r\n";
        if (value.IndexOfAny(SpecialChars.ToCharArray()) >= 0)
        {
            result = $"\"{result}\"";
        }

        // mitigate CSV injection
        // https://owasp.org/www-community/attacks/CSV_Injection
        var csvInjectionChars = new[] { '=', '+', '-', '@', 0x09, 0x0D };
        if (csvInjectionChars.Any(x => result.StartsWith(Convert.ToChar(x).ToString())))
        {
            result = $"'{result}";
        }

        // not rfc 4180, but remove all new lines since Excel can't read it correctly
        result = result.Replace(Environment.NewLine, " ").Replace('\n', ' ');

        return result;
    }
}