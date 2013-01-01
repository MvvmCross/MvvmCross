// MvxPhoneCallTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.WindowsPhone.Platform.Tasks;
using Microsoft.Phone.Tasks;

namespace Cirrious.MvvmCross.Plugins.PhoneCall.WindowsPhone
{
    public class MvxPhoneCallTask : MvxWindowsPhoneTask, IMvxPhoneCallTask
    {
        #region IMvxPhoneCallTask Members

        public void MakePhoneCall(string name, string number)
        {
            var pct = new PhoneCallTask {DisplayName = name, PhoneNumber = number};
            DoWithInvalidOperationProtection(pct.Show);
        }

        #endregion
    }
}