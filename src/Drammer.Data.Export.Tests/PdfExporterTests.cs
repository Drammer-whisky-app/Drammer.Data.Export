using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Drammer.Data.Export.Tests;

public sealed class PdfExporterTests
{
    private readonly Fixture _fixture = new ();

    public PdfExporterTests()
    {
        QuestPDF.Settings.License = LicenseType.Community; // license used for unit testing

        _fixture.Customize<decimal>(c => c.FromFactory<int>(x => x * 1.99m));
    }

    [Fact]
    public void Export_ReturnsDocument()
    {
        // Arrange
        var data = new DataTable<TextHeader>(new[] { new TextHeader("test1"), new TextHeader("test2"), new CurrencyNumericTextHeader("test3", "$") });

        for (var i = 0; i < 100; i++)
        {
            data.AddRow(new DataRow { _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<decimal>() });
        }

        var exporter = new PdfExporter(data, new PdfSettings
        {
            HeaderText = _fixture.Create<string>(),
            FooterText = x =>
            {
                x.Span(_fixture.Create<string>());
            }
        });

        // Act
        var result = exporter.Export();

        // Assert
        result.Should().NotBeNull();

        using var stream = new FileStream("document.pdf", FileMode.Create);
        result.GeneratePdf(stream);
    }
}