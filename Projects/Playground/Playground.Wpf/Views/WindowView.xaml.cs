using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Wpf.Views.Presenters.Attributes;

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
