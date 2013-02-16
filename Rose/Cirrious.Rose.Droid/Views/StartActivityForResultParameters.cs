using Android.Content;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class StartActivityForResultParameters
    {
        public StartActivityForResultParameters(Intent intent, int requestCode)
        {
            RequestCode = requestCode;
            Intent = intent;
        }

        public Intent Intent { get; private set; }
        public int RequestCode { get; private set; }    
    }
}