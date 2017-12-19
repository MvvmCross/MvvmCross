using MvvmCross.Core.Views;

namespace MvvmCross.Wpf.Views.Presenters.Attributes
{
    public class MvxContentPresentationAttribute : MvxBasePresentationAttribute
    {
        public string WindowIdentifier { get; set; }
        public bool StackNavigation { get; set; } = true;
    }
}
