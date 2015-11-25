// MvxPhoneCallTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.WindowsPhone.Tasks;
using Microsoft.Phone.Tasks;

namespace MvvmCross.Plugins.PhoneCall.WindowsPhone
{
    public class MvxPhoneCallTask : MvxWindowsPhoneTask, IMvxPhoneCallTask
    {
        public void MakePhoneCall(string name, string number)
        {
            var pct = new PhoneCallTask { DisplayName = name, PhoneNumber = number };
            DoWithInvalidOperationProtection(pct.Show);
        }
    }
}