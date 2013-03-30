// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Windows.System;

namespace Cirrious.MvvmCross.Plugins.Email.WindowsStore
{
    public class MvxComposeEmailTask : IMvxComposeEmailTask
    {
        public void ComposeEmail(string to, string cc, string subject, string body, bool isHtml)
        {
            // this is the best I can do so far... 
            // see - http://stackoverflow.com/questions/10674193/winrt-how-to-email-a-message-to-a-specific-person
            var uri = new Uri("mailto:" + to, UriKind.Absolute);
            Launcher.LaunchUriAsync(uri); 
        }
    }
}