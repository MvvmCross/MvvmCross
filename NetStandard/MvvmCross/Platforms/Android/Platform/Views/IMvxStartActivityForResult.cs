using Android.Content;

namespace MvvmCross.Platform.Droid.Views
{
    public interface IMvxStartActivityForResult
    {
        void MvxInternalStartActivityForResult(Intent intent, int requestCode);
    }
}