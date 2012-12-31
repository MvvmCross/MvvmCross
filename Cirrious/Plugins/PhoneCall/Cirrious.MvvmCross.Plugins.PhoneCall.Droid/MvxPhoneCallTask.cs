#region Copyright
// <copyright file="MvxPhoneCallTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Android.Content;
using Android.Net;
using Android.Telephony;
using Cirrious.MvvmCross.Droid.Platform.Tasks;

namespace Cirrious.MvvmCross.Plugins.PhoneCall.Droid
{
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