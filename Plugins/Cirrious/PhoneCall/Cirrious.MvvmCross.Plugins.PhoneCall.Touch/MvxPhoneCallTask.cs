// MvxPhoneCallTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Touch.Platform.Tasks;
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.Plugins.PhoneCall.Touch
{
    public class MvxPhoneCallTask : MvxTouchTask, IMvxPhoneCallTask
    {
        #region IMvxPhoneCallTask Members

        public void MakePhoneCall(string name, string number)
        {
            var url = new NSUrl("tel:" + number);
            DoUrlOpen(url);
        }

        #endregion
    }
}