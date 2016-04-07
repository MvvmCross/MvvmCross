using MvvmCross.Core.ViewModels;
using UIKit;

namespace MvvmCross.iOS.Support.SidePanels
{
    /// <summary>
    /// The Panel reset hint
    /// </summary>
    /// <seealso cref="MvxPresentationHint" />
    public class MvxPanelPushViewPresentationHint : MvxPresentationHint
    {
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
		/// <param name="panel">The panel.</param>
		public MvxPanelPushViewPresentationHint(UIViewController view)
		{
			ViewController = view;
		}
    }
}