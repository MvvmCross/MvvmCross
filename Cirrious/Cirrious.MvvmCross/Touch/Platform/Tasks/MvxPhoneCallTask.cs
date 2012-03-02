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
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.Touch.Platform.Tasks
{
    public class MvxPhoneCallTask : MvxTouchTask, IMvxPhoneCallTask
    {
        #region IMvxPhoneCallTask Members

        public void MakePhoneCall(string name, string number)
        {
#warning Should we do something about devices like ipod and ipad with no dialer!			
			var url = new NSUrl("tel:" + number);
            DoUrlOpen(url);
        }

        #endregion
    }
}