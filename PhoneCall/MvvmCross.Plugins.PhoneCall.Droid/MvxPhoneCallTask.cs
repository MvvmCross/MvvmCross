// MvxPhoneCallTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Net;
using Android.Telephony;
using MvvmCross.Platform.Droid.Platform;

namespace MvvmCross.Plugins.PhoneCall.Droid
{
    [Preserve(AllMembers = true)]
	public class MvxPhoneCallTask
        : MvxAndroidTask
          , IMvxPhoneCallTask
    {
        #region IMvxPhoneCallTask Members

        public void MakePhoneCall(string name, string number)
        {
            var phoneNumber = PhoneNumberUtils.FormatNumber(number);
            var newIntent = new Intent(Intent.ActionDial, Uri.Parse("tel:" + phoneNumber));
            StartActivity(newIntent);
        }

        #endregion
    }
}