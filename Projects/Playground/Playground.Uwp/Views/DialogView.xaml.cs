using MvvmCross.Platforms.Uap.Presenters.Attributes;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.Uwp.Views
{
    [MvxViewFor(typeof(ModalViewModel))]
    [MvxDialogViewPresentationAttribute]
    public sealed partial class DialogView : MvxWindowsContentDialog
    {
        public DialogView()
        {
            this.InitializeComponent();
        }
    }
}
