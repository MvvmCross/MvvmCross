using MvvmCross.Platforms.Uap.Presenters.Attributes;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.Uwp.Views
{
    [MvxViewFor(typeof(ModalViewModel))]
    [MvxModalViewPresentation]
    public sealed partial class ModalView : MvxWindowsContentDialog
    {
        public ModalView()
        {
            this.InitializeComponent();
        }
    }
}
