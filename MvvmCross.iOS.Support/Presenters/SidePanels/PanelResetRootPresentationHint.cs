namespace MvvmCross.iOS.Support.Presenters.SidePanels
{
    using Core.ViewModels;

    /// <summary>
    /// The Panel reset hint
    /// </summary>
    /// <seealso cref="MvxPresentationHint" />
    public class PanelResetRootPresentationHint : MvxPresentationHint
    {
        /// <summary>
        /// The panel
        /// </summary>
        public readonly PanelEnum Panel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelResetRootPresentationHint"/> class.
        /// </summary>
        /// <param name="panel">The panel.</param>
        public PanelResetRootPresentationHint(PanelEnum panel)
        {
            Panel = panel;
        }
    }
}
