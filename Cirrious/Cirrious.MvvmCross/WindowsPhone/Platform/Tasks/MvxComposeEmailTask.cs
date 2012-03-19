#region Copyright
// <copyright file="MvxComposeEmailTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.Platform.Tasks;
using Microsoft.Phone.Tasks;

namespace Cirrious.MvvmCross.WindowsPhone.Platform.Tasks
{
    public class MvxComposeEmailTask : MvxWindowsPhoneTask, IMvxComposeEmailTask
    {
        public void ComposeEmail(string to, string cc, string subject, string body, bool isHtml)
        {
            var task = new EmailComposeTask() { To = to, Subject = subject, Cc = cc, Body = body };
            DoWithInvalidOperationProtection(task.Show);
        }
    }
}