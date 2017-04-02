using System;
namespace MvvmCross.iOS.Views.Presenters.Attributes
{
    public class MvxTabPresentationAttribute : MvxBasePresentationAttribute
    {
        public string TabName { get; set; }

        public string TabIconName { get; set; }

        public bool WrapInNavigationController { get; set; }

        public string TabAccessibilityIdentifier { get; set; }
    }
}
