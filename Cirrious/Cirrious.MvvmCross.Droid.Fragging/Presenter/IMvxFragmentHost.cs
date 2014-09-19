using Android.OS;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Fragging.Presenter
{
    public interface IMvxFragmentHost
    {
        bool Show(MvxViewModelRequest request, Bundle bundle);
    }
}