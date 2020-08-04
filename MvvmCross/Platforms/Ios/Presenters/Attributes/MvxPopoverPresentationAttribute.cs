using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Platforms.Ios.Presenters.Attributes
{
    public class MvxPopoverPresentationAttribute : MvxBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = false;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
        
        public static CGSize DefaultPreferredContentSize = CGSize.Empty;
        public CGSize PreferredContentSize { get; set; } = DefaultPreferredContentSize;

        public static bool DefaultAnimated = true;
        public bool Animated { get; set; } = DefaultAnimated;
        
        public static UIPopoverArrowDirection DefaultPermittedArrowDirections = UIPopoverArrowDirection.Any;
        public UIPopoverArrowDirection PermittedArrowDirections { get; set; } = DefaultPermittedArrowDirections;
    }
}
