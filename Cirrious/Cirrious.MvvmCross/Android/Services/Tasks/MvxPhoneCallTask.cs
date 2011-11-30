#region Copyright

// <copyright file="MvxPhoneCallTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Android.Content;
using Android.Telephony;
using Cirrious.MvvmCross.Interfaces.Services.Tasks;

namespace Cirrious.MvvmCross.Android.Services.Tasks
{
    public class MvxPhoneCallTask : MvxWindowsPhoneTask, IMvxPhoneCallTask
    {
        #region IMvxPhoneCallTask Members

        public void MakePhoneCall(string name, string number)
        {
#warning What exceptions could be thrown here?
#warning Does this need to be on UI thread?
            var phoneNumber = PhoneNumberUtils.FormatNumber(number);
            var newIntent = new Intent(Intent.ActionDial, global::Android.Net.Uri.Parse("tel:" + phoneNumber));
            StartActivity(newIntent);
        }

        #endregion
    }
}