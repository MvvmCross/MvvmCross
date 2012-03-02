#region Copyright
// <copyright file="MvxPhoneCallTask.cs" company="Cirrious">
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
    public class MvxPhoneCallTask : MvxWindowsPhoneTask, IMvxPhoneCallTask
    {
        #region IMvxPhoneCallTask Members

        public void MakePhoneCall(string name, string number)
        {
            var pct = new PhoneCallTask {DisplayName = name, PhoneNumber = number};
            DoWithInvalidOperationProtection(pct.Show);
        }

        #endregion
    }
}