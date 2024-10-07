namespace Drammer.Data.Export;

/// <summary>
/// The data table.
/// </summary>
public sealed class DataTable<THeaderType>
    where THeaderType : IColumnHeader
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataTable{THeaderType}"/> class.
    /// </summary>
    /// <param name="headers">
    /// The headers.
    /// </param>
    public DataTable(IEnumerable<THeaderType>? headers)
    {
        Headers = headers?.ToList() ?? new List<THeaderType>();
        Rows = new List<DataRow>();
    }

    /// <summary>
    /// Gets the headers.
    /// </summary>
    public List<THeaderType> Headers { get; }

    /// <summary>
    /// Gets the rows.
    /// </summary>
    public List<DataRow> Rows { get; }

    /// <summary>
    /// Add a row.
    /// </summary>
    /// <param name="dataRow">
    /// The data row.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when number of headers and columns are not equal.
    /// </exception>
    public void AddRow(DataRow dataRow)
    {
        if (dataRow.Count != Headers.Count)
        {
            throw new ArgumentException($"Invalid data row; columns expected: {Headers.Count}, actual: {dataRow.Count}");
        }

        Rows.Add(dataRow);
    }
}