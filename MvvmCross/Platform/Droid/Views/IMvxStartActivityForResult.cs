namespace MvvmCross.Platform.Droid.Views
{
    using Android.Content;

    public interface IMvxStartActivityForResult
    {
        void MvxInternalStartActivityForResult(Intent intent, int requestCode);
    }
}