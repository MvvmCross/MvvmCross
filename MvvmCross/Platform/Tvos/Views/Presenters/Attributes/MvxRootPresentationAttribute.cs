using MvvmCross.Core.Views;

namespace MvvmCross.tvOS.Views.Presenters.Attributes
{
    public class MvxRootPresentationAttribute : MvxBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = false;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
    }
}
