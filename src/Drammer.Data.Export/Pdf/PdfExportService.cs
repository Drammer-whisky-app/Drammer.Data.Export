using System.Globalization;
using Drammer.Data.Export.Models;
using Microsoft.Extensions.Options;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Drammer.Data.Export.Pdf;

internal sealed class PdfExportService : IPdfExportService
{
    private readonly IOptions<PdfExportOptions> _options;

    public PdfExportService(IOptions<PdfExportOptions> options)
    {
        _options = options;
    }

    public Task<ExportResult> ExportWishListAsync(
        IEnumerable<WishListItem> data,
        CultureInfo cultureInfo,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(
            () =>
            {
                var creationDate = DateTimeOffset.UtcNow;
                var document = Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Size(PageSizes.A4.Landscape());
                            page.Margin(2, Unit.Centimetre);
                            page.PageColor(Colors.White);
                            page.DefaultTextStyle(x => x
                                .FontSize(_options.Value.FontSize)
                                .LineHeight(_options.Value.LineHeight)
                                .FontColor(_options.Value.DefaultFontColor));

                            page
                                .Header()
                                .Text(_options.Value.TitleWishListExport)
                                .SemiBold()
                                .FontSize(_options.Value.HeaderFontSize)
                                .FontColor(_options.Value.HeaderFontColor);

                            page.Content().Table(ExportWishList(data, cultureInfo));

                            page
                                .Footer()
                                .Row(r => FooterRow(r, creationDate, cultureInfo));
                        });
                    })
                    .WithMetadata(
                        GetDocumentMetadata(_options.Value.TitleWishListExport, _options.Value.Author, creationDate))
                    .WithSettings(GetDocumentSettings());

                return ToResult(document, creationDate);
            },
            cancellationToken);
    }

    public Task<ExportResult> ExportCollectionAsync(
        IEnumerable<CollectionItem> data,
        CultureInfo cultureInfo,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(
            () =>
            {
                var creationDate = DateTimeOffset.UtcNow;

                var document = Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Size(PageSizes.A4.Portrait());
                            page.Margin(2, Unit.Centimetre);
                            page.PageColor(Colors.White);
                            page.DefaultTextStyle(x => x
                                .FontSize(_options.Value.FontSize)
                                .LineHeight(_options.Value.LineHeight)
                                .FontColor(_options.Value.DefaultFontColor));

                            page
                                .Header()
                                .Text(_options.Value.TitleCollectionExport)
                                .SemiBold()
                                .FontSize(_options.Value.HeaderFontSize)
                                .FontColor(_options.Value.HeaderFontColor);

                            page.Content()
                                .PaddingTop(10)
                                .PaddingBottom(15)
                                .Column(column =>
                                {
                                    foreach (var d in data)
                                    {
                                        column.Item().Element(x => CollectionItemContent(x, d, cultureInfo));
                                        column.Item().PaddingVertical(10).LineHorizontal(1);
                                    }
                                });

                            page
                                .Footer()
                                .Row(r => FooterRow(r, creationDate, cultureInfo));
                        });
                    })
                    .WithMetadata(
                        GetDocumentMetadata(_options.Value.TitleCollectionExport, _options.Value.Author, creationDate))
                    .WithSettings(GetDocumentSettings());

                return ToResult(document, creationDate);
            },
            cancellationToken);
    }

    public Task<ExportResult> ExportCheckInsAsync(
        IEnumerable<CheckInItem> data,
        CultureInfo cultureInfo,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(
            () =>
            {
                var creationDate = DateTimeOffset.UtcNow;

                var document = Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Size(PageSizes.A4.Portrait());
                            page.Margin(2, Unit.Centimetre);
                            page.PageColor(Colors.White);
                            page.DefaultTextStyle(x => x
                                .FontSize(_options.Value.FontSize)
                                .FontFamily(_options.Value.FontFamily)
                                .LineHeight(_options.Value.LineHeight)
                                .FontColor(_options.Value.DefaultFontColor));

                            page
                                .Header()
                                .Text(_options.Value.TitleCheckInExport)
                                .SemiBold()
                                .FontSize(_options.Value.HeaderFontSize)
                                .FontColor(_options.Value.HeaderFontColor);

                            page.Content()
                                .PaddingTop(10)
                                .PaddingBottom(15)
                                .Column(column =>
                                {
                                    foreach (var d in data)
                                    {
                                        column.Item().Element(x => CheckInItemContent(x, d, cultureInfo));
                                        column.Item().PaddingVertical(10).LineHorizontal(1);
                                    }
                                });

                            page
                                .Footer()
                                .Row(r => FooterRow(r, creationDate, cultureInfo));
                        });
                    })
                    .WithMetadata(
                        GetDocumentMetadata(_options.Value.TitleCheckInExport, _options.Value.Author, creationDate))
                    .WithSettings(GetDocumentSettings());

                return ToResult(document, creationDate);
            },
            cancellationToken);
    }

    private static DocumentSettings GetDocumentSettings()
    {
        return new DocumentSettings
        {
            PdfA = true,
            CompressDocument = true,
            ContentDirection = ContentDirection.LeftToRight,
        };
    }

    private static DocumentMetadata GetDocumentMetadata(string title, string author, DateTimeOffset creationDate)
    {
        var defaultMetadata = DocumentMetadata.Default;
        defaultMetadata.CreationDate = creationDate;
        defaultMetadata.Author = author;
        defaultMetadata.Creator = author;
        defaultMetadata.Title = title;
        return defaultMetadata;
    }

    private static ExportResult ToResult(Document document, DateTimeOffset creationDate)
    {
        return new ExportResult
        {
            Data = document.GeneratePdf(),
            ContentType = ContentTypes.Pdf,
            CreatedAt = creationDate,
        };
    }

    private Action<TableDescriptor> ExportWishList(IEnumerable<WishListItem> data, CultureInfo cultureInfo)
    {
        return table =>
        {
            IContainer DefaultCellStyle(IContainer container, Color fontColor)
            {
                return container
                    .DefaultTextStyle(x => x.FontColor(fontColor).FontSize(_options.Value.FontSize));
            }

            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn();
                columns.ConstantColumn(80);
                columns.ConstantColumn(80);
                columns.ConstantColumn(40);
                columns.ConstantColumn(50);
            });

            table.Header(header =>
            {
                header.Cell().Element(HeaderCellStyle).Text("Bottling").SemiBold();
                header.Cell().Element(HeaderCellStyle).Text("Id").SemiBold();
                header.Cell().Element(HeaderCellStyle).Text("Date added").SemiBold();
                header.Cell().Element(HeaderCellStyle).Text("Rating").SemiBold();
                header.Cell().Element(HeaderCellStyle).Text("ABV").SemiBold();
                return;

                IContainer HeaderCellStyle(IContainer container) =>
                    DefaultCellStyle(container, _options.Value.TableHeaderFontColor);
            });

            foreach (var d in data)
            {
                table.Cell().Element(CellStyle).AlignLeft().Text(d.BottlingName);
                table.Cell().Element(CellStyle).AlignLeft().Text(d.DrmId);
                table.Cell().Element(CellStyle).AlignLeft().Text(d.DateAdded.ToShortDateValue(cultureInfo));
                table.Cell().Element(CellStyle).AlignRight().Text(d.Rating.ToStringValue(cultureInfo));
                table.Cell().Element(CellStyle).AlignRight().Text(d.Abv.HasValue? $"{d.Abv.ToStringValue(cultureInfo)}%" : string.Empty);
            }

            return;

            IContainer CellStyle(IContainer container) => DefaultCellStyle(container, _options.Value.DefaultFontColor);
        };
    }

    private void FooterRow(RowDescriptor row, DateTimeOffset creationDate, CultureInfo cultureInfo)
    {
        row.RelativeItem().AlignLeft().Text(Footer(creationDate, cultureInfo));
        row.ConstantItem(50).AlignRight().Text(x =>
        {
            x.DefaultTextStyle(y => y
                .FontSize(_options.Value.FooterFontSize)
                .FontColor(_options.Value.FooterFontColor));
            x.CurrentPageNumber();
            x.Span(" / ");
            x.TotalPages();
        });
    }

    private Action<TextDescriptor> Footer(DateTimeOffset creationDate, CultureInfo cultureInfo)
    {
        return text =>
        {
            var generatedAt =
                $"{creationDate.ToString(cultureInfo.DateTimeFormat.ShortDatePattern)} {creationDate.ToString(cultureInfo.DateTimeFormat.ShortTimePattern)} UTC";

            text
                .DefaultTextStyle(x => x
                    .FontSize(_options.Value.FooterFontSize)
                    .FontColor(_options.Value.FooterFontColor));

            text.Span($"{string.Format(_options.Value.FooterText, generatedAt)} - ");
            text.Hyperlink(_options.Value.Url, _options.Value.Url);
        };
    }

    private void CollectionItemContent(IContainer container, CollectionItem data, CultureInfo cultureInfo)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(100);
                columns.RelativeColumn();
            });

            table.Cell().Text("Bottling");
            table.Cell().Text(data.BottlingName);

            table.Cell().Text("Id");
            table.Cell().Text(data.DrmId);

            if (data.Abv is > 0)
            {
                table.Cell().Text("ABV");
                table.Cell().Text($"{data.Abv.ToStringValue(cultureInfo)}%");
            }

            table.Cell().Text("Date added");
            table.Cell().Text(data.DateAdded.ToShortDateValue(cultureInfo));

            if (data.AmountLeftInBottle is >= 0)
            {
                table.Cell().Text("Amount left");
                table.Cell().Text($"{data.AmountLeftInBottle.ToStringValue(cultureInfo)} ml");
            }

            if (data.PriceOfPurchase is >= 0)
            {
                if (!string.IsNullOrWhiteSpace(data.Currency))
                {
                    table.Cell().Text("Price");
                    table.Cell().Text($"{data.Currency} {data.PriceOfPurchase.ToStringValue(cultureInfo)}");
                }
                else
                {
                    table.Cell().Text("Price");
                    table.Cell().Text(data.PriceOfPurchase.ToStringValue(cultureInfo));
                }
            }

            if (!string.IsNullOrWhiteSpace(data.Store))
            {
                table.Cell().Text("Store");
                table.Cell().Text(data.Store);
            }

            if (!string.IsNullOrWhiteSpace(data.Location))
            {
                table.Cell().Text("Store");
                table.Cell().Text(data.Location);
            }

            if (!string.IsNullOrWhiteSpace(data.Note))
            {
                table.Cell().Text("Note");
                table.Cell().Text(data.Note);
            }

            if (data.TastingNote?.Rating is > 0)
            {
                table.Cell().Text("Rating");
                table.Cell().Text(data.TastingNote.Rating.ToStringValue(cultureInfo, 1));
            }

            if (!string.IsNullOrWhiteSpace(data.TastingNote?.Nose))
            {
                table.Cell().Text("Nose");
                table.Cell().Text(data.TastingNote.Nose);
            }

            if (!string.IsNullOrWhiteSpace(data.TastingNote?.Palate))
            {
                table.Cell().Text("Palate");
                table.Cell().Text(data.TastingNote.Palate);
            }

            if (!string.IsNullOrWhiteSpace(data.TastingNote?.Finish))
            {
                table.Cell().Text("Finish");
                table.Cell().Text(data.TastingNote.Finish);
            }

            if (!string.IsNullOrWhiteSpace(data.TastingNote?.Conclusion))
            {
                table.Cell().Text("Conclusion");
                table.Cell().Text(data.TastingNote.Conclusion);
            }
        });
    }

    private void CheckInItemContent(IContainer container, CheckInItem data, CultureInfo cultureInfo)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(100);
                columns.RelativeColumn();
            });

            if (!string.IsNullOrWhiteSpace(data.BottlingName))
            {
                table.Cell().Text("Bottling");
                table.Cell().Text(data.BottlingName);
            }

            if (!string.IsNullOrWhiteSpace(data.DrmId))
            {
                table.Cell().Text("Id");
                table.Cell().Text(data.DrmId);
            }

            if (!string.IsNullOrWhiteSpace(data.Location))
            {
                table.Cell().Text("Location");
                table.Cell().Text(data.Location);
            }

            table.Cell().Text("Date");
            table.Cell().Text(data.CheckInDate.ToShortDateValue(cultureInfo));

            if (data.YearBottled is > 0)
            {
                table.Cell().Text("Year bottled");
                table.Cell().Text(data.YearBottled.ToStringValue(cultureInfo));
            }

            if (data.Abv is > 0)
            {
                table.Cell().Text("ABV");
                table.Cell().Text($"{data.Abv.ToStringValue(cultureInfo)}%");
            }

            if (data.TastingNote?.Rating is > 0)
            {
                table.Cell().Text("Rating");
                table.Cell().Text(data.TastingNote.Rating.ToStringValue(cultureInfo, 1));
            }

            if (!string.IsNullOrWhiteSpace(data.TastingNote?.Nose))
            {
                table.Cell().Text("Nose");
                table.Cell().Text(data.TastingNote.Nose);
            }

            if (!string.IsNullOrWhiteSpace(data.TastingNote?.Palate))
            {
                table.Cell().Text("Palate");
                table.Cell().Text(data.TastingNote.Palate);
            }

            if (!string.IsNullOrWhiteSpace(data.TastingNote?.Finish))
            {
                table.Cell().Text("Finish");
                table.Cell().Text(data.TastingNote.Finish);
            }

            if (!string.IsNullOrWhiteSpace(data.TastingNote?.Conclusion))
            {
                table.Cell().Text("Conclusion");
                table.Cell().Text(data.TastingNote.Conclusion);
            }
        });
    }
}