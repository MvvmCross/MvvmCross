using Android.App;
using Android.Content;

namespace Cirrious.CrossCore.Droid.Views
{
    public class MvxActivityResultParameters
    {
        public MvxActivityResultParameters(int requestCode, Result resultCode, Intent data)
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