// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace MvvmCross.Plugin.Email.Platforms.Wpf
{
    public class MvxComposeEmailTask : IMvxComposeEmailTask
    {
        public void ComposeEmail(string to, string cc = null, string subject = null, string body = null,
            bool isHtml = false, string dialogTitle = null)
        {
            // this is a simple attempt - using the 'mailto:' url
            var url = new MvxMailToUrlBuilder().Build(to, cc, subject, body);
            Process.Start(new ProcessStartInfo(url));
        }
    }
}
