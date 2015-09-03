// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Diagnostics;

namespace Cirrious.MvvmCross.Plugins.Email.Wpf
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
