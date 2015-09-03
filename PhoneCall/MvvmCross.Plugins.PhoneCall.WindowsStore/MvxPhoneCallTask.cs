// MvxPhoneCallTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Windows.System;

namespace MvvmCross.Plugins.PhoneCall.WindowsStore
{
    public class MvxPhoneCallTask : IMvxPhoneCallTask
    {
        public void MakePhoneCall(string name, string number)
        {
            //The tel URI for Telephone Numbers : http://tools.ietf.org/html/rfc3966
            //Handled by skype
            var uri = new Uri("tel:" + Uri.EscapeDataString(number), UriKind.Absolute);
            Launcher.LaunchUriAsync(uri);
        }
    }
}
