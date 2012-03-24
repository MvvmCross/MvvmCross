using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.Conference.UI.Droid.Views
{
    public interface IBaseView<TViewModel>
        : IMvxView<TViewModel>
        where TViewModel : BaseViewModel
    {
        // just a marker interface
    }
}