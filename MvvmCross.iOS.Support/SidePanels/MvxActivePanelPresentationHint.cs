using MvvmCross.Core.ViewModels;

namespace MvvmCross.iOS.Support.SidePanels
{
    /// <summary>
    /// Determines which Panel the next ShowViewModel call will act upon
    /// and optionally allows you to show that panel (default) or not
    /// </summary>
    public class MvxActivePanelPresentationHint : MvxPresentationHint
    {
        public readonly MvxPanelEnum ActivePanel;
        public readonly bool ShowPanel;

        public MvxActivePanelPresentationHint(MvxPanelEnum activePanel, bool showPanel = true)
        {
            ActivePanel = activePanel;
            ShowPanel = showPanel;
        }
    }
}