#nullable enable
using MvvmCross.ViewModels;
using Microsoft.UI.Xaml;
using System.Collections.Concurrent;
using MvvmCross.Platforms.WinUi.Views;

namespace MvvmCross.Platforms.WinUi.Presenters.Models
{
    /// <summary>
    /// Holds information regarding the different windows.
    /// </summary>
    public class WindowInformation
    {
        private readonly List<IMvxViewModel> _subViewModels = new();

        /// <summary>
        /// Initializes a new instance of the WindowInformation class.
        /// </summary>
        /// <param name="window">The Window.</param>
        /// <param name="rootFrame">The root frame of the window.</param>
        /// <param name="viewModel">The viewmodel belonging to the root frame.</param>
        public WindowInformation(Window window, IMvxWindowsFrame rootFrame, IMvxViewModel? viewModel)
        {
            this.Window = window;
            this.RootFrame = rootFrame;
            this.ViewModel = viewModel;
        }

        /// <summary>
        /// Gets or sets the Window.
        /// </summary>
        public Window Window { get; }

        /// <summary>
        /// Gets or sets the root frame belonging to this window.
        /// </summary>
        public IMvxWindowsFrame RootFrame { get; }

        /// <summary>
        /// Gets or sets the ViewModel belonging to this window.
        /// </summary>
        public IMvxViewModel? ViewModel { get; }

        /// <summary>
        /// Registers the given viewmodel to a specific key (usually region name).
        /// </summary>
        /// <param name="viewModel">the viewmodel to register.</param>
        public void RegisterSubViewModel(IMvxViewModel viewModel)
        {
            if (!this._subViewModels.Contains(viewModel))
            {
                this._subViewModels.Add(viewModel);
            }
        }

        /// <summary>
        /// Removes the viewmodel registration.
        /// </summary>
        /// <param name="viewModel">The viewmodel to remove the viewmodel registration for.</param>
        public void UnregisterSubViewModel(IMvxViewModel viewModel)
        {
            this._subViewModels.Remove(viewModel);
        }

        /// <summary>
        /// Checks if this instance belongs to the given Window.
        /// </summary>
        /// <param name="w">The window to check against.</param>
        /// <returns>True if it is a match.</returns>
        public bool IsFor(Window w)
        {
            return this.Window == w;
        }

        /// <summary>
        /// Checks if this instance or any of the registered subViewModels belongs to the given viewmodel.
        /// </summary>
        /// <param name="viewModel">The viewmodel to check against.</param>
        /// <returns>True if it is a match.</returns>
        public bool IsFor(IMvxViewModel viewModel)
        {
            return this.ViewModel == viewModel || this._subViewModels.Any(v => v == viewModel);
        }
    }
}

