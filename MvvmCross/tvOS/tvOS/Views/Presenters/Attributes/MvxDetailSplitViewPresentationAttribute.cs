using MvvmCross.Core.Views;
namespace MvvmCross.tvOS.Views.Presenters.Attributes
{
    public class MvxDetailSplitViewPresentationAttribute : MvxBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = true;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
    }
}
