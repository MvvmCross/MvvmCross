using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.FullFragging.Presenter
{
    public interface IMvxFragmentsPresenter
    {
        void RegisterViewModelAtHost<TViewModel>(IMvxFragmentHost host) 
            where TViewModel : IMvxViewModel;

        void UnRegisterViewModelAtHost<TViewModel>() 
            where TViewModel : IMvxViewModel;
    }
}