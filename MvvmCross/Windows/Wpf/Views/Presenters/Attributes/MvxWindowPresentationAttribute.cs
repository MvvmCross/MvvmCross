using MvvmCross.Core.Views;

namespace MvvmCross.Wpf.Views.Presenters.Attributes
{
    public class MvxWindowPresentationAttribute : MvxBasePresentationAttribute
    {
        public string Identifier { get; set; }
        public bool Modal { get; set; }
    }
}
