using System.Text;

namespace Drammer.Data.Export;

/// <summary>
/// The ExportableAsync interface.
/// </summary>
public interface IExportableAsync : IExportable
{
    /// <summary>
    /// The export to file async.
    /// </summary>
    /// <param name="encoding">
    /// The encoding.
    /// </param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<byte[]> ExportToFileAsync(Encoding? encoding = null, CancellationToken cancellationToken = default);
}