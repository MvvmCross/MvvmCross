#nullable enable
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace MvvmCross.Platforms.WinUi.Presenters.Models;

/// <summary>
/// Interface for Window root views to allow the View Presenter to set the current appwindow.
/// </summary>
public interface IMvxNeedWindow
{
    /// <summary>
    /// Sets the AppWindow for the current view.
    /// </summary>
    /// <param name="window">The window.</param>
    /// <param name="appWindow">The app window instance.</param>
    public void SetWindow(Window window, AppWindow appWindow);

    /// <summary>
    /// Executed when the close button is clicked. Return true if the window can be closed, false if it cannot be closed.
    /// </summary>
    /// <returns>A bool indicating true if the window can be closed.</returns>
    public bool CanClose();
}
