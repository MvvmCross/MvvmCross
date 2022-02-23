// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.System;

namespace MvvmCross.Plugin.PhoneCall.Platforms.Uap
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
