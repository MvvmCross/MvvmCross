using MvvmCross.Core.ViewModels;

namespace MvvmCross.iOS.Support.SidePanels
{
    /// <summary>
    /// The Panel Pop To Root hint.
    /// </summary>
    /// <seealso cref="MvxPresentationHint" />
    public class MvxPanelPopToRootPresentationHint : MvxPresentationHint
    {
        /// <summary>
        /// The panel
        /// </summary>
        public readonly MvxPanelEnum Panel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvxPanelPopToRootPresentationHint"/> class.
        /// </summary>
        /// <param name="panel">The panel.</param>
        public MvxPanelPopToRootPresentationHint(MvxPanelEnum panel)
        {
            Panel = panel;
        }
    }
}