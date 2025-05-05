namespace Drammer.Data.Export;

public sealed class ExportResult
{
    public required byte[] Data { get; init; }

    public required string ContentType { get; init; }

    public required DateTimeOffset CreatedAt { get; init; }
}