namespace Drammer.Data.Export.Models;

public sealed record WishListItem
{
    public required string BottlingName { get; init; }

    public decimal? Rating { get; init; }

    public DateTimeOffset? DateAdded { get; init; }

    public decimal? Abv { get; init; }
}