namespace Drammer.Data.Export.Models;

public sealed record TastingNote
{
    public decimal? Rating { get; init; }

    public string? Nose { get; init; }

    public string? Palate { get; init; }

    public string? Finish { get; init; }

    public string? Conclusion { get; init; }
}