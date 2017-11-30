using MvvmCross.Core.Views;

namespace MvvmCross.tvOS.Views.Presenters.Attributes
{
    public class MvxChildPresentationAttribute : MvxBasePresentationAttribute
    {
        public static bool DefaultAnimated = true;
        public bool Animated { get; set; } = DefaultAnimated;
    }
}
