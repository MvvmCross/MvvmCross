// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Windows.System;

namespace MvvmCross.Plugins.Email.WindowsStore
{
    // IMvxComposeEmailTaskEx not supported currently in WinStore 
    // to support this we'd need to experiment with multiple addresses (e.g. see http://www.sightspecific.com/~mosh/www_faq/multrec.html)
    public class MvxComposeEmailTask 
        : IMvxComposeEmailTask
    {
        public void ComposeEmail(string to, string cc = null, string subject = null, string body = null,
            bool isHtml = false, string dialogTitle = null)
        {
            // this is the best I can do so far... 
            // see - http://stackoverflow.com/questions/10674193/winrt-how-to-email-a-message-to-a-specific-person
            var url = new MvxMailToUrlBuilder().Build(to, cc, subject, body);
            var uri = new Uri(url, UriKind.Absolute);
            Launcher.LaunchUriAsync(uri); 
        }
    }
}
