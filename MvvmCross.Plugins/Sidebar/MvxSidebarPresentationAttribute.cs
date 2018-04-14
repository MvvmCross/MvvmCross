// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Plugin.Sidebar
{
    public class MvxSidebarPresentationAttribute : MvxBasePresentationAttribute
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
        /// Animate the View Controller
        /// </summary>
        public readonly bool Animated;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvxSidebarPresentationAttribute"/> class.
        /// </summary>
        /// <param name="panel">The panel.</param>
        /// <param name="hintType">Type of the hint.</param>
        /// <param name="showPanel">if set to <c>true</c> [show panel].</param>
        /// <param name="behaviour">The splitview behaviour value</param>
        /// <param name="animated">if set to <c>true</c> animate the View Controller.</param>
        public MvxSidebarPresentationAttribute(MvxPanelEnum panel, MvxPanelHintType hintType, bool showPanel, MvxSplitViewBehaviour behaviour = MvxSplitViewBehaviour.None, bool animated = true)
        {
            Panel = panel;
            ShowPanel = showPanel;
            HintType = hintType;
            SplitViewBehaviour = behaviour;
            Animated = animated;
        }
    }
}
