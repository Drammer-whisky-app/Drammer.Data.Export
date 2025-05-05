namespace Drammer.Data.Export;

public sealed class CsvExportOptions
{
    public const string SectionName = nameof(CsvExportOptions);

    public string Delimiter { get; set; } = ";";
}