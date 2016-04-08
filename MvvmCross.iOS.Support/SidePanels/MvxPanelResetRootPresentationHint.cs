using MvvmCross.Core.ViewModels;
using UIKit;

namespace MvvmCross.iOS.Support.SidePanels
{
    /// <summary>
    /// The Panel reset hint
    /// </summary>
    /// <seealso cref="MvxPresentationHint" />
    public class MvxPanelResetRootPresentationHint : MvxPresentationHint
    {
        /// <summary>
        /// The panel
        /// </summary>
        public readonly MvxPanelEnum Panel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvxPanelResetRootPresentationHint"/> class.
        /// </summary>
        /// <param name="panel">The panel.</param>
        public MvxPanelResetRootPresentationHint(MvxPanelEnum panel)
        {
            Panel = panel;
        }

    }
}