using System.Text;

namespace Drammer.Data.Export;

/// <summary>
/// The Exportable interface.
/// </summary>
public interface IExportable
{
    /// <summary>
    /// The export to file.
    /// </summary>
    /// <param name="encoding">
    /// The encoding.
    /// </param>
    /// <returns>
    /// The <see cref="byte"/> array.
    /// </returns>
    byte[] ExportToFile(Encoding? encoding = null);
}