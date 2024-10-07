namespace Drammer.Data.Export;

/// <summary>
/// The header.
/// </summary>
public record DefaultHeader : IColumnHeader
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultHeader"/> class.
    /// </summary>
    /// <param name="title">
    /// The title.
    /// </param>
    public DefaultHeader(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentNullException(nameof(title));
        }

        Title = title;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultHeader"/> class.
    /// </summary>
    /// <param name="title">
    /// The title.
    /// </param>
    /// <param name="formatter">
    /// The formatter.
    /// </param>
    public DefaultHeader(string title, Func<object, string>? formatter)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentNullException(nameof(title));
        }

        Title = title;
        Formatter = formatter;
    }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets or sets the formatter.
    /// </summary>
    public Func<object, string>? Formatter { get; }

    /// <summary>
    /// Formats a value.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public virtual string FormatValue(object? value)
    {
        if (Formatter != null && value != null)
        {
            return Formatter(value);
        }

        return value?.ToString() ?? string.Empty;
    }
}