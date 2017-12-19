// MvxPhoneCallTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Net;
using Android.OS;
using Android.Telephony;
using Java.Util;
using MvvmCross.Platform.Droid.Platform;

namespace MvvmCross.Plugins.PhoneCall.Droid
{
    [Preserve(AllMembers = true)]
	public class MvxPhoneCallTask
        : MvxAndroidTask, IMvxPhoneCallTask
    {
        public void MakePhoneCall(string name, string number)
        {
            string phoneNumber;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                phoneNumber = PhoneNumberUtils.FormatNumber(number, Locale.GetDefault(Locale.Category.Format).Country);
            else if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                phoneNumber = PhoneNumberUtils.FormatNumber(number, Locale.Default.Country);
            else
#pragma warning disable 618
                phoneNumber = PhoneNumberUtils.FormatNumber(number);
#pragma warning restore 618

            var newIntent = new Intent(Intent.ActionDial, Uri.Parse("tel:" + phoneNumber));
            StartActivity(newIntent);
        }
    }
}
