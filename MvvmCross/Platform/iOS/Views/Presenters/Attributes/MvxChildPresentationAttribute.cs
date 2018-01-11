using MvvmCross.Core.Views;

namespace MvvmCross.iOS.Views.Presenters.Attributes
{
    public class MvxChildPresentationAttribute : MvxBasePresentationAttribute
    {
        public static bool DefaultAnimated = true;
        public bool Animated { get; set; } = DefaultAnimated;
    }
}
