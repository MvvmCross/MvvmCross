namespace MvvmCross.iOS.Support.Presenters.SidePanels
{
    using Core.ViewModels;

    /// <summary>
    /// The Panel Pop To Root hint.
    /// </summary>
    /// <seealso cref="MvxPresentationHint" />
    public class PanelPopToRootPresentationHint : MvxPresentationHint
    {
        /// <summary>
        /// The panel
        /// </summary>
        public readonly PanelEnum Panel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelPopToRootPresentationHint"/> class.
        /// </summary>
        /// <param name="panel">The panel.</param>
        public PanelPopToRootPresentationHint(PanelEnum panel)
        {
            Panel = panel;
        }
    }
}
