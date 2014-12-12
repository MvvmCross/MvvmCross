using System;
using Windows.UI.Xaml.Navigation;

namespace Cirrious.MvvmCross.WindowsCommon.Views
{
    /// <summary>
    /// Event arguments for the navigating from event. 
    /// </summary>
    public class MvxWindowsNavigatingCancelEventArgs
    {
        /// <summary>
        /// Gets or sets a value indiciating whether the navigation should be cancelled. 
        /// </summary>
        public bool Cancel { get; set; }
        /// <summary>
        /// Gets the page object which is involved in the navigation. 
        /// </summary>
        public object Content { get; internal set; }
        /// <summary>
        /// Gets the navigation mode. 
        /// </summary>
        public NavigationMode NavigationMode { get; internal set; }
        /// <summary>
        /// Gets the type of the page. 
        /// </summary>
        public Type SourcePageType { get; internal set; }
    }
}