// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Windows.System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Plugins.Email.WindowsStore
{
    public class MvxComposeEmailTask : IMvxComposeEmailTask
    {
        public void ComposeEmail(string to, string cc, string subject, string body, bool isHtml)
        {
            // this is the best I can do so far... 
            // see - http://stackoverflow.com/questions/10674193/winrt-how-to-email-a-message-to-a-specific-person
            var url = new MvxMailToUrlBuilder().Build(to, cc, subject, body);
            var uri = new Uri(url, UriKind.Absolute);
            Launcher.LaunchUriAsync(uri); 
        }

		public void ComposeEmail(
			string[] to, string[] cc, string subject, 
			string body, bool isHtml, 
			List<EmailAttachment> attachments)
		{
			throw new NotImplementedException();
		}

		public bool CanSendEmail()
		{
			return true;
		}

    }
}
