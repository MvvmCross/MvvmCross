using Cirrious.MvvmCross.ViewModels;
using Xamarin.Forms;

namespace Cirrious.MvvmCross.Forms.Presenter.Core
{
    public interface IMvxFormsPageLoader
    {
        Page LoadPage(MvxViewModelRequest request);
    }
}