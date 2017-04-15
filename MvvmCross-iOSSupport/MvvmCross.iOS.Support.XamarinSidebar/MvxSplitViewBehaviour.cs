namespace MvvmCross.iOS.Support.XamarinSidebar
{
    /// <summary>
    /// Denotes where this view should be displayed when the application is running on an iPad
    /// in a UISplitView container
    /// </summary>
    public enum MvxSplitViewBehaviour
    {
        /// <summary>
        /// Default non split view behaviour
        /// </summary>
        None,

        /// <summary>
        /// Show this view in the master panel
        /// </summary>
        Master,

        /// <summary>
        /// Show this view in the detail panel
        /// </summary>
        Detail
    }
}