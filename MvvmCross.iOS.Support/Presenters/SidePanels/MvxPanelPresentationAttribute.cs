namespace MvvmCross.iOS.Support.Presenters.SidePanels
{
    using System;

    /// <summary>
    /// The panel presentation hint
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class MvxPanelPresentationAttribute : Attribute
    {
        /// <summary>
        /// The hint type
        /// </summary>
        public readonly MvxPanelHintType HintType;

        /// <summary>
        /// The panel
        /// </summary>
        public readonly MvxPanelEnum Panel;

        /// <summary>
        /// The split view behaviour
        /// </summary>
        public readonly MvxSplitViewBehaviour SplitViewBehaviour;

        /// <summary>
        /// Show the panel
        /// </summary>
        public readonly bool ShowPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvxPanelPresentationAttribute"/> class.
        /// </summary>
        /// <param name="panel">The panel.</param>
        /// <param name="hintType">Type of the hint.</param>
        /// <param name="showPanel">if set to <c>true</c> [show panel].</param>
        public MvxPanelPresentationAttribute(MvxPanelEnum panel, MvxPanelHintType hintType, bool showPanel, MvxSplitViewBehaviour behaviour = MvxSplitViewBehaviour.None)
        {
            Panel = panel;
            ShowPanel = showPanel;
            HintType = hintType;
            SplitViewBehaviour = behaviour;
        }
    }
}