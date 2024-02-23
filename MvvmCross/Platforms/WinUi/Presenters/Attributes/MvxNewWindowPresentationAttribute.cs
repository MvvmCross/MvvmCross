namespace MvvmCross.Platforms.WinUi.Presenters.Attributes;

    using MvvmCross.Presenters.Attributes;

/// <summary>
/// Attribute to indicate that a view should be shown in a new window.
/// When using this attribute make sure not to do any long operation that after completion will update the UI.
/// </summary>
public class MvxNewWindowPresentationAttribute : MvxBasePresentationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MvxNewWindowPresentationAttribute"/> class.
    /// </summary>
    public MvxNewWindowPresentationAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MvxNewWindowPresentationAttribute"/> class.
    /// </summary>
    /// <param name="width">The start width of the window.</param>
    /// <param name="height">The start height of the window.</param>
    public MvxNewWindowPresentationAttribute(int width, int height)
    {
        this.Width = width;
        this.Height = height;
    }

    /// <summary>
    /// Gets the width.
    /// </summary>
    public int? Width { get; }

    /// <summary>
    /// Gets the height.
    /// </summary>
    public int? Height { get; }
}
