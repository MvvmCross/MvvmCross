// MvxPhoneCallTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.iOS.Platform;
using Foundation;

namespace MvvmCross.Plugins.PhoneCall.iOS
{
    public class MvxPhoneCallTask : MvxIosTask, IMvxPhoneCallTask
    {
        #region IMvxPhoneCallTask Members

        public void MakePhoneCall(string name, string number)
        {
            var url = new NSUrl("tel:" + number);
            DoUrlOpen(url);
        }

        #endregion IMvxPhoneCallTask Members
    }
}