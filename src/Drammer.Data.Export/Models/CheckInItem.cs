namespace Drammer.Data.Export.Models;

public sealed record CheckInItem
{
    public string? BottlingName { get; init; }

    public string? CaskNumber { get; init; }

    public int? YearBottled { get; init; }

    public required DateTimeOffset CheckInDate { get; init; }

    public TastingNote? TastingNote { get; init; }

    public decimal? Abv { get; init; }

    public string? Location { get; init; }
}