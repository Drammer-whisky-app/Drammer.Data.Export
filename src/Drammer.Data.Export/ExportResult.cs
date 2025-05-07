namespace Drammer.Data.Export;

public sealed class ExportResult
{
    /// <summary>
    /// Gets the data.
    /// </summary>
    public required byte[] Data { get; init; }

    /// <summary>
    /// Gets the content type.
    /// </summary>
    public required string ContentType { get; init; }

    /// <summary>
    /// Gets the creation date.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; init; }
}