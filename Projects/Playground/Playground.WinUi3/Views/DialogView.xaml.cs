using MvvmCross.Platforms.WinUi.Presenters.Attributes;
using MvvmCross.Platforms.WinUi.Views;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.WinUi.Views
{
    [MvxViewFor(typeof(ModalViewModel))]
    [MvxDialogViewPresentation]
    public sealed partial class DialogView : DialogViewBase
    {
        public DialogView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class DialogViewBase : MvxWindowsContentDialog<ModalViewModel>
    {
    }
}
