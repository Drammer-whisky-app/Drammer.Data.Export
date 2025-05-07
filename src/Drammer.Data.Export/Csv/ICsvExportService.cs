using System.Globalization;
using System.Text;
using Drammer.Data.Export.Models;

namespace Drammer.Data.Export.Csv;

public interface ICsvExportService
{
    Task<ExportResult> ExportWishListAsync(
        IEnumerable<WishListItem> data,
        CultureInfo cultureInfo,
        Encoding? encoding = null,
        CancellationToken cancellationToken = default);

    Task<ExportResult> ExportCollectionAsync(
        IEnumerable<CollectionItem> data,
        CultureInfo cultureInfo,
        Encoding? encoding = null,
        CancellationToken cancellationToken = default);

    Task<ExportResult> ExportCheckInsAsync(
        IEnumerable<CheckInItem> data,
        CultureInfo cultureInfo,
        Encoding? encoding = null,
        CancellationToken cancellationToken = default);
}