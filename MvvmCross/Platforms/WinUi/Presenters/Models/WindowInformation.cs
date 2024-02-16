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
        private readonly ConcurrentDictionary<string, IMvxViewModel> _subViewModels = new();

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
        /// <param name="key">The key to register the subview model under.</param>
        /// <param name="viewModel">the viewmodel to register.</param>
        public void RegisterSubViewModel(string key, IMvxViewModel viewModel)
        {
            this._subViewModels.AddOrUpdate(key, viewModel, (_,_) => viewModel);
        }

        /// <summary>
        /// Gets a list of sub view models.
        /// </summary>
        /// <returns>The list of subview models currently registered.</returns>
        public List<IMvxViewModel> GetSubViewModels()
        {
            return this._subViewModels.Values.ToList();
        }

        /// <summary>
        /// Removes the viewmodel registration.
        /// </summary>
        /// <param name="key">The key to remove the viewmodel registration for.</param>
        public void UnregisterSubViewModel(string key)
        {
            this._subViewModels.TryRemove(key, out _);
        }

        /// <summary>
        /// Removes the viewmodel registration.
        /// </summary>
        /// <param name="viewModel">The viewmodel to remove the viewmodel registration for.</param>
        public void UnregisterSubViewModel(IMvxViewModel viewModel)
        {
            var key = this._subViewModels.FirstOrDefault(v => v.Value == viewModel).Key;
            if (key != null)
            {
                this.UnregisterSubViewModel(key);
            }
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
            return this.ViewModel == viewModel || this._subViewModels.Any(v => v.Value == viewModel);
        }
    }
}

