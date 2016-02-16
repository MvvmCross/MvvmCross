namespace MvvmCross.iOS.Support.Presenters.SidePanels
{
    using System;

    /// <summary>
    /// The panel presentation hint
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class PanelPresentationAttribute : Attribute
    {
        /// <summary>
        /// The hint type
        /// </summary>
        public readonly PanelHintType HintType;

        /// <summary>
        /// The panel
        /// </summary>
        public readonly PanelEnum Panel;

        /// <summary>
        /// The split view behaviour
        /// </summary>
        public readonly SplitViewBehaviour SplitViewBehaviour;

        /// <summary>
        /// Show the panel
        /// </summary>
        public readonly bool ShowPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelPresentationAttribute"/> class.
        /// </summary>
        /// <param name="panel">The panel.</param>
        /// <param name="hintType">Type of the hint.</param>
        /// <param name="showPanel">if set to <c>true</c> [show panel].</param>
        public PanelPresentationAttribute(PanelEnum panel, PanelHintType hintType, bool showPanel, SplitViewBehaviour behaviour = SplitViewBehaviour.None)
        {
            Panel = panel;
            ShowPanel = showPanel;
            HintType = hintType;
            SplitViewBehaviour = behaviour;
        }
    }
}