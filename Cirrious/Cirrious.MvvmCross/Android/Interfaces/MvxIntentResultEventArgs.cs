using System;
using Android.App;
using Android.Content;

namespace Cirrious.MvvmCross.Android.Interfaces
{
    public class MvxIntentResultEventArgs 
        : EventArgs
    {
        public MvxIntentResultEventArgs(int requestCode, Result resultCode, Intent data)
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