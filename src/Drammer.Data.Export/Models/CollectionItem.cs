namespace Drammer.Data.Export.Models;

public sealed record CollectionItem
{
    public required string BottlingName { get; init; }

    public required string DrmId { get; init; }

    public DateTimeOffset? DateAdded { get; init; }

    public int? AmountLeftInBottle { get; init; }

    public string? Store { get; init; }

    public decimal? PriceOfPurchase { get; init; }

    public string? Currency { get; init; }

    public string? Note { get; init; }

    public TastingNote? TastingNote { get; init; }

    public decimal? Abv { get; init; }

    public string? Location { get; init; }
}