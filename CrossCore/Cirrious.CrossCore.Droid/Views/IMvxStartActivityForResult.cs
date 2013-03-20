using Android.Content;

namespace Cirrious.CrossCore.Droid.Views
{
    public interface IMvxStartActivityForResult
    {
        void MvxInternalStartActivityForResult(Intent intent, int requestCode);
    }
}