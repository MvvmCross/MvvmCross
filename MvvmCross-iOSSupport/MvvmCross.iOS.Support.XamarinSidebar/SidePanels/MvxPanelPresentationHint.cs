namespace MvvmCross.iOS.Support.SidePanels
{
    using Core.ViewModels;

    public abstract class MvxPanelPresentationHint : MvxPresentationHint
    {
        protected readonly MvxPanelEnum Panel;

        public MvxPanelPresentationHint(MvxPanelEnum panel)
        {
            Panel = panel;
        }

        public abstract bool Navigate();
    }
}