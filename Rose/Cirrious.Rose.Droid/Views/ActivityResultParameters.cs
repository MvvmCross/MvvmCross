using Android.App;
using Android.Content;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class ActivityResultParameters
    {
        public ActivityResultParameters(int requestCode, Result resultCode, Intent data)
        {
            Data = data;
            ResultCode = resultCode;
            RequestCode = requestCode;
        }

        public int RequestCode { get; private set; }
        public Result ResultCode { get; private set; }
        public Intent Data { get; private set; }
    }
}