using MvvmCross.Core.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenter.Core
{
    public interface IMvxFormsPageLoader
    {
        Page LoadPage(MvxViewModelRequest request);
    }
}