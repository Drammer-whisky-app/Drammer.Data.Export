using System.Globalization;
using System.Text;
using Drammer.Data.Export.Models;

namespace Drammer.Data.Export.Pdf;

public interface IPdfExportService
{
    Task<ExportResult> ExportWishListAsync(
        IEnumerable<WishListItem> data,
        CultureInfo cultureInfo,
        CancellationToken cancellationToken = default);

    Task<ExportResult> ExportCollectionAsync(
        IEnumerable<CollectionItem> data,
        CultureInfo cultureInfo,
        CancellationToken cancellationToken = default);

    Task<ExportResult> ExportCheckInsAsync(
        IEnumerable<CheckInItem> data,
        CultureInfo cultureInfo,
        CancellationToken cancellationToken = default);
}