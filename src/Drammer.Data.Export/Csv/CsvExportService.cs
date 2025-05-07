using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Drammer.Data.Export.Models;
using Microsoft.Extensions.Options;

namespace Drammer.Data.Export.Csv;

internal sealed class CsvExportService : ICsvExportService
{
    private readonly IOptions<CsvExportOptions> _options;

    public CsvExportService(IOptions<CsvExportOptions> options)
    {
        _options = options;
    }

    public async Task<ExportResult> ExportWishListAsync(
        IEnumerable<WishListItem> data,
        CultureInfo cultureInfo,
        Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        var csvData = ExportWishList(data, cultureInfo);
        return new ExportResult
        {
            Data = await StringToByteArrayAsync(csvData, encoding, cancellationToken).ConfigureAwait(false),
            ContentType = ContentTypes.Csv,
            CreatedAt = DateTimeOffset.UtcNow,
        };
    }

    public async Task<ExportResult> ExportCollectionAsync(
        IEnumerable<CollectionItem> data,
        CultureInfo cultureInfo,
        Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        var csvData = ExportCollection(data, cultureInfo);
        return new ExportResult
        {
            Data = await StringToByteArrayAsync(csvData, encoding, cancellationToken).ConfigureAwait(false),
            ContentType = ContentTypes.Csv,
            CreatedAt = DateTimeOffset.UtcNow,
        };
    }

    public async Task<ExportResult> ExportCheckInsAsync(
        IEnumerable<CheckInItem> data,
        CultureInfo cultureInfo,
        Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        var csvData = ExportCheckIns(data, cultureInfo);
        return new ExportResult
        {
            Data = await StringToByteArrayAsync(csvData, encoding, cancellationToken).ConfigureAwait(false),
            ContentType = ContentTypes.Csv,
            CreatedAt = DateTimeOffset.UtcNow,
        };
    }

    private static async Task<byte[]> StringToByteArrayAsync(
        string data,
        Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        using var ms = new MemoryStream();
        var writer = new StreamWriter(ms, encoding ?? Encoding.Default);
        await writer.WriteAsync(data).ConfigureAwait(false);
        await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
        ms.Position = 0;
        return ms.ToArray();
    }

    private string ExportWishList(IEnumerable<WishListItem> data, CultureInfo cultureInfo)
    {
        var sb = new StringBuilder();

        // header
        var header = new HashSet<string>
        {
            "Bottling",
            "Id",
            "Rating",
            "Date added",
            "ABV"
        };
        sb.AppendLine(string.Join(_options.Value.Delimiter, header.Select(ApplyRfc4180)));

        // content
        foreach (var d in data)
        {
            List<string> row =
            [
                d.BottlingName,
                d.DrmId,
                d.Rating.ToStringValue(cultureInfo) ?? string.Empty,
                d.DateAdded.ToShortDateValue(cultureInfo) ?? string.Empty,
                d.Abv.ToStringValue(cultureInfo) ?? string.Empty
            ];

            sb.AppendLine(string.Join(_options.Value.Delimiter, row.Select(ApplyRfc4180)));
        }

        return sb.ToString();
    }

    private string ExportCollection(IEnumerable<CollectionItem> data, CultureInfo cultureInfo)
    {
        var sb = new StringBuilder();

        // header
        var header = new HashSet<string>
        {
            "Bottling",
            "Id",
            "Date in collection",
            "Amount left in bottle (ml)",
            "Store",
            "Price",
            "Currency",
            "Note",
            "Rating",
            "Nose",
            "Palate",
            "Finish",
            "Conclusion",
            "Abv",
            "Store location",
        };
        sb.AppendLine(string.Join(_options.Value.Delimiter, header.Select(ApplyRfc4180)));

        // content
        foreach (var d in data)
        {
            List<string> row =
            [
                d.BottlingName,
                d.DrmId,
                d.DateAdded.ToShortDateValue(cultureInfo) ?? string.Empty,
                d.AmountLeftInBottle.ToStringValue(cultureInfo) ?? string.Empty,
                d.Store ?? string.Empty,
                d.PriceOfPurchase.ToStringValue(cultureInfo, 2, true) ?? string.Empty,
                d.Currency ?? string.Empty,
                d.Note ?? string.Empty,
                d.TastingNote?.Rating.ToStringValue(cultureInfo, 1) ?? string.Empty,
                d.TastingNote?.Nose ?? string.Empty,
                d.TastingNote?.Palate ?? string.Empty,
                d.TastingNote?.Finish ?? string.Empty,
                d.TastingNote?.Conclusion ?? string.Empty,
                d.Abv.ToStringValue(cultureInfo) ?? string.Empty,
                d.Location ?? string.Empty,
            ];

            sb.AppendLine(string.Join(_options.Value.Delimiter, row.Select(ApplyRfc4180)));
        }

        return sb.ToString();
    }

    private string ExportCheckIns(IEnumerable<CheckInItem> data, CultureInfo cultureInfo)
    {
        var sb = new StringBuilder();

        // header
        var header = new HashSet<string>
        {
            "Bottling",
            "Id",
            "Rating",
            "Year bottled",
            "Review date",
            "Nose",
            "Palate",
            "Finish",
            "Conclusion",
            "Abv",
            "Location",
        };
        sb.AppendLine(string.Join(_options.Value.Delimiter, header.Select(ApplyRfc4180)));

        // content
        foreach (var d in data)
        {
            List<string> row =
            [
                d.BottlingName ?? string.Empty,
                d.DrmId ?? string.Empty,
                d.TastingNote?.Rating.ToStringValue(cultureInfo, 1) ?? string.Empty,
                d.YearBottled?.ToString() ?? string.Empty,
                d.CheckInDate.ToShortDateValue(cultureInfo),
                d.TastingNote?.Nose ?? string.Empty,
                d.TastingNote?.Palate ?? string.Empty,
                d.TastingNote?.Finish ?? string.Empty,
                d.TastingNote?.Conclusion ?? string.Empty,
                d.Abv.ToStringValue(cultureInfo) ?? string.Empty,
                d.Location ?? string.Empty,
            ];

            sb.AppendLine(string.Join(_options.Value.Delimiter, row.Select(ApplyRfc4180)));
        }

        return sb.ToString();
    }

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