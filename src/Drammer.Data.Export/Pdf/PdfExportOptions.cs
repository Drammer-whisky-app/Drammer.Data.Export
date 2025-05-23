﻿using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Drammer.Data.Export.Pdf;

public sealed class PdfExportOptions
{
    public const string SectionName = nameof(PdfExportOptions);

    public string Author { get; init; } = "Drammer Whisky app";

    public string Url { get; init; } = "https://drammer.com/";

    public string FooterText {get; init; } = "Generated by Drammer Whisky app at {0}";

    public string TitleWishListExport { get; init; } = "Wish List Export";

    public string TitleCollectionExport { get; init; } = "Collection Export";

    public string TitleCheckInExport { get; init; } = "Check-in Export";

    public string FontFamily { get; init; } = "Lato";

    public Color DefaultFontColor { get; init; } = Colors.Black;

    public Color TableHeaderFontColor { get; init; } = Colors.Black;

    public Color HeaderFontColor { get; init; } = Colors.Black;

    public Color FooterFontColor { get; init; } = Colors.Black;

    public int FontSize { get; init; } = 12;

    public float LineHeight { get; init; } = 1.7f;

    public int HeaderFontSize { get; init; } = 20;

    public int FooterFontSize { get; init; } = 8;
}