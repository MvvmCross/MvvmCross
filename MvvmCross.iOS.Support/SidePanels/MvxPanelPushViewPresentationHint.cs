namespace MvvmCross.iOS.Support.SidePanels
{
    using Core.ViewModels;
    using UIKit;

    /// <summary>
    /// The Panel reset hint
    /// </summary>
    /// <seealso cref="MvxPresentationHint" />
    public class MvxPanelPushViewPresentationHint : MvxPresentationHint
    {
		/// <summary>
		/// The viewcontroller thats has to be pushed
		/// </summary>
		public UIViewController ViewController;

        /// <summary>
        /// The panel
        /// </summary>
        public readonly MvxPanelEnum Panel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvxPanelResetRootPresentationHint"/> class.
        /// </summary>
        /// <param name="panel">The panel.</param>
		public MvxPanelPushViewPresentationHint(MvxPanelEnum panel)
        {
            Panel = panel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MvxPanelResetRootPresentationHint"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public MvxPanelPushViewPresentationHint(UIViewController view)
		{
			ViewController = view;
		}
    }
}