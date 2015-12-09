// MvxStartActivityForResultParameters.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Droid.Views
{
    using Android.Content;

    public class MvxStartActivityForResultParameters
    {
        public MvxStartActivityForResultParameters(Intent intent, int requestCode)
        {
            this.RequestCode = requestCode;
            this.Intent = intent;
        }

        public Intent Intent { get; private set; }
        public int RequestCode { get; private set; }
    }
}