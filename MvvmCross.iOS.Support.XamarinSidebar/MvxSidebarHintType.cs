using System;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public enum MvxSidebarHintType
    {
        /// <summary>
        /// Indicates that the view is a master view and as such should display the sidebar toggle
        /// on the navigation bar. 
        /// </summary>
        Master,

        /// <summary>
        /// Indicates that the view is a detail view and as such should be pushed on the navigation view controller.
        /// </summary>
        Detail
    }
}

