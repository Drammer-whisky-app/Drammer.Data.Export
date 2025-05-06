using System.Globalization;
using Drammer.Data.Export.Models;
using Drammer.Data.Export.Pdf;
using Microsoft.Extensions.Options;
using QuestPDF.Infrastructure;

namespace Drammer.Data.Export.Tests.Pdf;

public sealed class PdfExportServiceTests
{
    private readonly Fixture _fixture = new ();

    public PdfExportServiceTests()
    {
        _fixture.Customize<decimal>(c => c.FromFactory<int>(x => x * 1.99m * Random.Shared.Next(1, 10)));

        QuestPDF.Settings.License = LicenseType.Community; // license used for unit testing
    }

    [Theory]
    [InlineData("nl")]
    [InlineData("us")]
    public async Task ExportWishListAsync_ReturnsResult(string culture)
    {
        // Arrange
        var exportService = new PdfExportService(Options.Create(new PdfExportOptions()));
        var wishListItems = _fixture.CreateMany<WishListItem>(Random.Shared.Next(1, 50)).ToList();

        // Act
        var result = await exportService.ExportWishListAsync(wishListItems, new CultureInfo(culture));

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNullOrEmpty();
        result.ContentType.Should().Be("application/pdf");

        ////using var stream = new FileStream("document.pdf", FileMode.Create);
        ////await stream.WriteAsync(result.Data);
    }

    [Theory]
    [InlineData("nl")]
    [InlineData("us")]
    public async Task ExportCollectionAsync_ReturnsResult(string culture)
    {
        // Arrange
        var exportService = new PdfExportService(Options.Create(new PdfExportOptions()));
        var data = _fixture.CreateMany<CollectionItem>(Random.Shared.Next(1, 50)).ToList();

        // Act
        var result = await exportService.ExportCollectionAsync(data, new CultureInfo(culture));

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNullOrEmpty();
        result.ContentType.Should().Be("application/pdf");
    }

    [Theory]
    [InlineData("nl")]
    [InlineData("us")]
    public async Task ExportCheckInsAsync_ReturnsResult(string culture)
    {
        // Arrange
        var exportService = new PdfExportService(Options.Create(new PdfExportOptions()));
        var data = _fixture.CreateMany<CheckInItem>(Random.Shared.Next(1, 50)).ToList();

        // Act
        var result = await exportService.ExportCheckInsAsync(data, new CultureInfo(culture));

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNullOrEmpty();
        result.ContentType.Should().Be("application/pdf");
    }
}