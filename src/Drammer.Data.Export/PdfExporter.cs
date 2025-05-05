using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Drammer.Data.Export;

public sealed class PdfExporter : ExporterBase<Document, TextHeader>, IExportableAsync
{
    private readonly PdfSettings _settings;
    private readonly DocumentMetadata? _metaData;

    public PdfExporter(DataTable<TextHeader> data, PdfSettings settings, DocumentMetadata? metaData = null)
    {
        _settings = settings;
        _metaData = metaData;
        DataTable = data;
    }

    public override Document Export()
    {
        if (DataTable == null)
        {
            throw new ArgumentException("DataTable cannot be null.");
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x
                    .FontSize(_settings.FontSize)
                    .FontColor(_settings.DefaultFontColor));

                page
                    .Header()
                    .Text(_settings.HeaderText)
                    .SemiBold()
                    .FontSize(_settings.HeaderFontSize)
                    .FontColor(_settings.HeaderFontColor);

                page.Content().Table(GetTableDescriptor(DataTable));

                page
                    .Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text
                            .DefaultTextStyle(x => x
                                .FontSize(_settings.FooterFontSize)
                                .FontColor(_settings.FooterFontColor));

                        _settings.FooterText(text);
                    });
            });
        })
            .WithMetadata(_metaData ?? DocumentMetadata.Default)
            .WithSettings(new DocumentSettings
            {
                PdfA = true,
                CompressDocument = true,
                ContentDirection = ContentDirection.LeftToRight,
            });

        return document;
    }

    public byte[] ExportToFile(Encoding? encoding = null)
    {
        var document = Export();
        return document.GeneratePdf();
    }

    public Task<byte[]> ExportToFileAsync(Encoding? encoding = null, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => ExportToFile(encoding), cancellationToken);
    }

    private Action<TableDescriptor> GetTableDescriptor(DataTable<TextHeader> dataTable)
    {
        return table =>
        {
            IContainer DefaultCellStyle(IContainer container, Color fontColor)
            {
                return container
                    .DefaultTextStyle(x => x.FontColor(fontColor).FontSize(_settings.FontSize));
            }

            table.ColumnsDefinition(columns =>
            {
                foreach (var header in dataTable.Headers)
                {
                    columns.RelativeColumn();
                }
            });

            table.Header(header =>
            {
                IContainer CellStyle(IContainer container) =>
                    DefaultCellStyle(container, _settings.TableHeaderFontColor)
                        .PaddingVertical(4)
                        .PaddingHorizontal(8);;

                foreach(var h in dataTable.Headers)
                {
                    header.Cell().Element(CellStyle).Text(h.Title).SemiBold();
                }
            });

            IContainer CellStyle(IContainer container) => DefaultCellStyle(container, _settings.DefaultFontColor)
                .PaddingVertical(4)
                .PaddingHorizontal(8);

            foreach (var row in dataTable.Rows)
            {
                var columns = ApplyFormat(row);
                for (var y = 0; y < columns.Count; y++)
                {
                    var value = !string.IsNullOrEmpty(columns[y]) ? columns[y] : string.Empty;

                    if (dataTable.Headers[y].Alignment == HeaderTitleAlignment.Right)
                    {
                        table.Cell().Element(CellStyle).AlignRight().Text(value);
                    }
                    else
                    {
                        table.Cell().Element(CellStyle).AlignLeft().Text(value);
                    }
                }
            }
        };
    }
}