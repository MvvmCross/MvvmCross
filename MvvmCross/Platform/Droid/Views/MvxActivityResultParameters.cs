// MvxActivityResultParameters.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Droid.Views
{
    using Android.App;
    using Android.Content;

    public class MvxActivityResultParameters
    {
        public MvxActivityResultParameters(int requestCode, Result resultCode, Intent data)
        {
            this.Data = data;
            this.ResultCode = resultCode;
            this.RequestCode = requestCode;
        }

        public int RequestCode { get; private set; }
        public Result ResultCode { get; private set; }
        public Intent Data { get; private set; }
    }
}