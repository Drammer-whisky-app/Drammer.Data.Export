using System.Globalization;
using System.Text;
using Drammer.Data.Export.Csv;
using Drammer.Data.Export.Models;
using Microsoft.Extensions.Options;

namespace Drammer.Data.Export.Tests.Csv;

public sealed class CsvExportServiceTests
{
    private readonly Fixture _fixture = new ();

    public CsvExportServiceTests()
    {
        _fixture.Customize<decimal>(c => c.FromFactory<int>(x => x * 1.99m * Random.Shared.Next(1, 10)));
    }

    [Theory]
    [InlineData("nl")]
    [InlineData("us")]
    public async Task ExportWishlistAsync_ReturnsData(string culture)
    {
        // Arrange
        var exportService = new CsvExportService(Options.Create(new CsvExportOptions()));
        var wishListItems = _fixture.CreateMany<WishListItem>(Random.Shared.Next(1, 50)).ToList();

        // Act
        var result = await exportService.ExportWishListAsync(wishListItems, new CultureInfo(culture));

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNullOrEmpty();
        result.ContentType.Should().Be("text/csv");

        var fileContents = GetFileContents(result);
        fileContents.Should().Contain("Bottling;Rating;Date added;ABV");
        CountLines(fileContents).Should().Be(wishListItems.Count + 1);
    }

    [Theory]
    [InlineData("nl")]
    [InlineData("us")]
    public async Task ExportCollectionAsync_ReturnsData(string culture)
    {
        // Arrange
        var exportService = new CsvExportService(Options.Create(new CsvExportOptions()));
        var data = _fixture.CreateMany<CollectionItem>(Random.Shared.Next(1, 50)).ToList();

        // Act
        var result = await exportService.ExportCollectionAsync(data, new CultureInfo(culture));

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNullOrEmpty();
        result.ContentType.Should().Be("text/csv");

        var fileContents = GetFileContents(result);
        CountLines(fileContents).Should().Be(data.Count + 1);
    }

    [Theory]
    [InlineData("nl")]
    [InlineData("us")]
    public async Task ExportCheckInsAsync_ReturnsData(string culture)
    {
        // Arrange
        var exportService = new CsvExportService(Options.Create(new CsvExportOptions()));
        var data = _fixture.CreateMany<CheckInItem>(Random.Shared.Next(1, 50)).ToList();

        // Act
        var result = await exportService.ExportCheckInsAsync(data, new CultureInfo(culture));

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNullOrEmpty();
        result.ContentType.Should().Be("text/csv");

        var fileContents = GetFileContents(result);
        CountLines(fileContents).Should().Be(data.Count + 1);
    }

    private static string GetFileContents(ExportResult result) => Encoding.Default.GetString(result.Data);

    private static int CountLines(string data) => data.Split(["\r\n", "\r", "\n"], StringSplitOptions.RemoveEmptyEntries).Length;
}