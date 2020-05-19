using MvvmCross.Platforms.Uap.Presenters.Attributes;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.Uwp.Views
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
