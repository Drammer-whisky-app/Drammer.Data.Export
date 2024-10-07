namespace Drammer.Data.Export;

/// <summary>
/// The text header.
/// </summary>
public record TextHeader : DefaultHeader
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextHeader"/> class.
    /// </summary>
    /// <param name="title">
    /// The title.
    /// </param>
    /// <param name="alignment">
    /// The alignment.
    /// </param>
    public TextHeader(string title, HeaderTitleAlignment alignment = HeaderTitleAlignment.Left)
        : base(title)
    {
        Alignment = alignment;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextHeader"/> class.
    /// </summary>
    /// <param name="title">
    /// The title.
    /// </param>
    /// <param name="formatter">
    /// The formatter.
    /// </param>
    /// <param name="alignment">
    /// The alignment.
    /// </param>
    public TextHeader(string title, Func<object, string>? formatter, HeaderTitleAlignment alignment = HeaderTitleAlignment.Left)
        : base(title, formatter)
    {
        Alignment = alignment;
    }

    /// <summary>
    /// Gets or sets the alignment.
    /// </summary>
    public HeaderTitleAlignment Alignment { get; }
}