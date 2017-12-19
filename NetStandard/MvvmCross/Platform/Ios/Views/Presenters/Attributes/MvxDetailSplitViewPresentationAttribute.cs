using MvvmCross.Core.Views;

namespace MvvmCross.iOS.Views.Presenters.Attributes
{
    public class MvxDetailSplitViewPresentationAttribute : MvxBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = false;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
    }
}
