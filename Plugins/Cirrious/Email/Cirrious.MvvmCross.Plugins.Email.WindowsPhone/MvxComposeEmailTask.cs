// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.WindowsPhone.Tasks;
using Microsoft.Phone.Tasks;

namespace Cirrious.MvvmCross.Plugins.Email.WindowsPhone
{
    public class MvxComposeEmailTask : MvxWindowsPhoneTask, IMvxComposeEmailTask
    {
        public void ComposeEmail(string to, string cc, string subject, string body, bool isHtml)
        {
            var task = new EmailComposeTask {To = to, Subject = subject, Cc = cc, Body = body};
            DoWithInvalidOperationProtection(task.Show);
        }
    }
}