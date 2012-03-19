#region Copyright
// <copyright file="MvxShareTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Interfaces.Platform.Tasks;
using Microsoft.Phone.Tasks;

namespace Cirrious.MvvmCross.WindowsPhone.Platform.Tasks
{
    public class MvxShareTask : MvxWindowsPhoneTask, IMvxShareTask
    {
        public void ShareShort(string message)
        {
            var task = new ShareStatusTask() {Status = message};
            DoWithInvalidOperationProtection(task.Show);
        }

        public void ShareLink(string title, string message, string link)
        {
            var task = new ShareLinkTask {  Title = title, Message = message, LinkUri = new Uri(link) };
            DoWithInvalidOperationProtection(task.Show);
        }
    }
}