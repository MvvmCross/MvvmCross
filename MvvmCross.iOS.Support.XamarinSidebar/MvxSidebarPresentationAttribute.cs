using System;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public class MvxSidebarPresentationAttribute : Attribute
    {
        public MvxSidebarPresentationAttribute(MvxSidebarHintType hintType, bool hideNavigationBar = false)
        {
            HintType = hintType;
            HideNavigationBar = hideNavigationBar;
        }

        public readonly MvxSidebarHintType HintType;
        public readonly bool HideNavigationBar;
    }
}

