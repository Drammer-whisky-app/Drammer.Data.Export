namespace Drammer.Data.Export;

/// <summary>
/// The ColumnHeader interface.
/// </summary>
public interface IColumnHeader
{
    /// <summary>
    /// Gets the title.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Gets the formatter.
    /// </summary>
    Func<object, string>? Formatter { get; }

    /// <summary>
    /// Formats a value.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    string FormatValue(object? value);
}