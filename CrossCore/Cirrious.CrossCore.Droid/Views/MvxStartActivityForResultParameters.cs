// MvxStartActivityForResultParameters.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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