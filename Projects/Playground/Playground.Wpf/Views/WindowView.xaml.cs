using MvvmCross.Platform.Wpf.Presenters.Attributes;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace Playground.Wpf.Views
{
    public partial class WindowView : IMvxOverridePresentationAttribute
    {
        public WindowView()
        {
            InitializeComponent();
        }

        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            return new MvxWindowPresentationAttribute
            {
                Identifier = $"{nameof(WindowView)}"
            };
        }
    }
}
