using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.iOS.Support.SidePanels
{
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

