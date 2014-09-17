// MvxPhoneCallTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Windows.System;

namespace Cirrious.MvvmCross.Plugins.PhoneCall.WindowsPhoneStore
{
    public class MvxPhoneCallTask : IMvxPhoneCallTask
    {
        public void MakePhoneCall(string name, string number)
        {
            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(number, name);
        }
    }
}