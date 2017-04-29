using MvvmCross.Core.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenters
{
    public interface IMvxFormsPageLoader
    {
        Page LoadPage(MvxViewModelRequest request);
    }
}