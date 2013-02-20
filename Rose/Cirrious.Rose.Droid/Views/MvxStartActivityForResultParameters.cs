using Android.Content;

namespace Cirrious.CrossCore.Droid.Views
{
    public class MvxStartActivityForResultParameters
    {
        public MvxStartActivityForResultParameters(Intent intent, int requestCode)
        {
            RequestCode = requestCode;
            Intent = intent;
        }

        public Intent Intent { get; private set; }
        public int RequestCode { get; private set; }    
    }
}