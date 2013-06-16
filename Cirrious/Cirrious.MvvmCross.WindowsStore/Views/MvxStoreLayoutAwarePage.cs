// MvxWinRTPage.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WindowsStore.Views
{
    /// <summary>
    ///     Windows Store Page is aware of changes in the application's view state and will go to the 
    ///     appropriate visual state when the view state changes
    /// </summary>
    [WebHostHidden]
    public class MvxStoreLayoutAwarePage : MvxStorePage
    {
        private List<Control> _layoutAwareControls;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MvxStoreLayoutAwarePage" /> class.
        /// </summary>
        public MvxStoreLayoutAwarePage()
        {
            if (DesignMode.DesignModeEnabled)
                return;

            Loaded += (sender, e) =>
            {
                StartLayoutUpdates(sender, e);
            };

            // Undo the same changes when the page is no longer visible
            Unloaded += (sender, e) =>
            {
                StopLayoutUpdates(sender, e);
            };
        }

        /// <summary>
        ///     Translates <see cref="ApplicationViewState" /> values into strings for visual state
        ///     management within the page.  The default implementation uses the names of enum values.
        ///     Subclasses may override this method to control the mapping scheme used.
        /// </summary>
        /// <param name="viewState">View state for which a visual state is desired.</param>
        /// <returns>
        ///     Visual state name used to drive the
        ///     <see cref="VisualStateManager" />
        /// </returns>
        /// <seealso cref="InvalidateVisualState" />
        protected virtual string DetermineVisualState(ApplicationViewState viewState)
        {
            return viewState.ToString();
        }

        /// <summary>
        ///     Updates all controls that are listening for visual state changes with the correct
        ///     visual state.
        /// </summary>
        /// <remarks>
        ///     Typically used in conjunction with overriding <see cref="DetermineVisualState" /> to
        ///     signal that a different value may be returned even though the view state has not
        ///     changed.
        /// </remarks>
        public void InvalidateVisualState()
        {
            if (_layoutAwareControls != null)
            {
                string visualState = DetermineVisualState(ApplicationView.Value);
                foreach (Control layoutAwareControl in _layoutAwareControls)
                {
                    VisualStateManager.GoToState(layoutAwareControl, visualState, false);
                }
            }
        }

        /// <summary>
        ///     Invoked as an event handler, typically on the <see cref="FrameworkElement.Loaded" />
        ///     event of a <see cref="Control" /> within the page, to indicate that the sender should
        ///     start receiving visual state management changes that correspond to application view
        ///     state changes.
        /// </summary>
        /// <param name="sender">
        ///     Instance of <see cref="Control" /> that supports visual state
        ///     management corresponding to view states.
        /// </param>
        /// <param name="e">Event data that describes how the request was made.</param>
        /// <remarks>
        ///     The current view state will immediately be used to set the corresponding
        ///     visual state when layout updates are requested.  A corresponding
        ///     <see cref="FrameworkElement.Unloaded" /> event handler connected to
        ///     <see cref="StopLayoutUpdates" /> is strongly encouraged.  Instances of
        ///     <see cref="MvxStoreLayoutAwarePage" /> automatically invoke these handlers in their Loaded and
        ///     Unloaded events.
        /// </remarks>
        /// <seealso cref="DetermineVisualState" />
        /// <seealso cref="InvalidateVisualState" />
        public void StartLayoutUpdates(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            if (control == null)
                return;
            if (_layoutAwareControls == null)
            {
                // Start listening to view state changes when there are controls interested in updates
                Window.Current.SizeChanged += WindowSizeChanged;
                _layoutAwareControls = new List<Control>();
            }
            _layoutAwareControls.Add(control);

            // Set the initial visual state of the control
            VisualStateManager.GoToState(control, DetermineVisualState(ApplicationView.Value), false);
        }

        /// <summary>
        ///     Invoked as an event handler, typically on the <see cref="FrameworkElement.Unloaded" />
        ///     event of a <see cref="Control" />, to indicate that the sender should start receiving
        ///     visual state management changes that correspond to application view state changes.
        /// </summary>
        /// <param name="sender">
        ///     Instance of <see cref="Control" /> that supports visual state
        ///     management corresponding to view states.
        /// </param>
        /// <param name="e">Event data that describes how the request was made.</param>
        /// <remarks>
        ///     The current view state will immediately be used to set the corresponding
        ///     visual state when layout updates are requested.
        /// </remarks>
        /// <seealso cref="StartLayoutUpdates" />
        public void StopLayoutUpdates(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            if (control == null || _layoutAwareControls == null)
                return;
            _layoutAwareControls.Remove(control);
            if (_layoutAwareControls.Count == 0)
            {
                // Stop listening to view state changes when no controls are interested in updates
                _layoutAwareControls = null;
                Window.Current.SizeChanged -= WindowSizeChanged;
            }
        }

        private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            InvalidateVisualState();
        }
    }
}