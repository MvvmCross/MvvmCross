#region Copyright
// <copyright file="MvxPhoneCallTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.Platform.Tasks;

namespace Cirrious.MvvmCross.Console.Services.Tasks
{
    public class MvxPhoneCallTask : IMvxPhoneCallTask
    {
        #region IMvxPhoneCallTask Members

        public void MakePhoneCall(string name, string number)
        {
            System.Console.WriteLine("YOUR JOB - Phone {0} on {1}", name, number);
        }

        #endregion
    }
}