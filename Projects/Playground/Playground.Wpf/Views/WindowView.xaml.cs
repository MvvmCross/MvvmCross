using MvvmCross.Platform.Wpf.Presenters.Attributes;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;

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
