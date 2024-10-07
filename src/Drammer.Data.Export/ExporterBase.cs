namespace Drammer.Data.Export;

/// <summary>
/// The exporter base.
/// </summary>
/// <typeparam name="TExportType">
/// The export type.
/// </typeparam>
/// <typeparam name="THeaderType">
/// The header type.
/// </typeparam>
public abstract class ExporterBase<TExportType, THeaderType>
    where TExportType : class
    where THeaderType : IColumnHeader
{
    /// <summary>
    /// Gets or sets the data table.
    /// </summary>
    public DataTable<THeaderType>? DataTable { get; protected set; }

    /// <summary>
    /// The export function.
    /// </summary>
    /// <returns>
    /// The <see cref="TExportType"/>.
    /// </returns>
    public abstract TExportType Export();

    /// <summary>
    /// Apply the value format.
    /// </summary>
    /// <param name="dataRow">
    /// The data row.
    /// </param>
    /// <returns>
    /// The <see cref="List{T}"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when lists are not equal in length.
    /// </exception>
    protected virtual List<string> ApplyFormat(List<object?> dataRow)
    {
        if (DataTable == null)
        {
            throw new ArgumentException("DataTable cannot be null.");
        }

        if (dataRow.Count != DataTable.Headers.Count)
        {
            throw new ArgumentException($"Items in dataRow ({dataRow.Count}) and header ({DataTable.Headers.Count}) are not equal.");
        }

        return dataRow.Select((t, i) => DataTable.Headers[i].FormatValue(t)).ToList();
    }
}