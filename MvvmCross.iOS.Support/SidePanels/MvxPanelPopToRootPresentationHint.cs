namespace MvvmCross.iOS.Support.SidePanels
{
    using Core.ViewModels;
    using UIKit;

    public class MvxPanelPopToRootPresentationHint: MvxPresentationHint
	{
		public UIViewController ViewController { get; set; }

		/// <summary>
		/// The panel
		/// </summary>
		public readonly MvxPanelEnum Panel;

		/// <summary>
		/// Initializes a new instance of the <see cref="MvxPanelResetRootPresentationHint"/> class.
		/// </summary>
		/// <param name="panel">The panel.</param>
		public MvxPanelPopToRootPresentationHint(MvxPanelEnum panel)
		{
			Panel = panel;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="MvxPanelResetRootPresentationHint"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public MvxPanelPopToRootPresentationHint(UIViewController view)
		{
			ViewController = view;
		}
	}
}