using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Drammer.Data.Export;

public sealed class PdfSettings
{
    public required string HeaderText { get; init; }

    public required Action<TextDescriptor> FooterText { get; init; }

    public Color DefaultFontColor { get; init; } = Colors.Black;

    public Color TableHeaderFontColor { get; init; } = Colors.Black;

    public Color HeaderFontColor { get; init; } = Colors.Black;

    public Color FooterFontColor { get; init; } = Colors.Black;

    public int FontSize { get; init; } = 12;
    public int HeaderFontSize { get; init; } = 20;
    public int FooterFontSize { get; init; } = 8;
}