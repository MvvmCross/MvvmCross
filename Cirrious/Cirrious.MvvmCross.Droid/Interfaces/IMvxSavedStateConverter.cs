using Android.OS;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Droid.Views
{
    public interface IMvxSavedStateConverter
    {
        IMvxBundle Read(Bundle bundle);
        void Write(Bundle bundle, IMvxBundle savedState);
    }
}