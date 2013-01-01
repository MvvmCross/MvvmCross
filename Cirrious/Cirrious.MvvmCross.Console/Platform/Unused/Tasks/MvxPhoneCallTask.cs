// MvxPhoneCallTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#if false

#warning removed as its not really useful currently

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

#endif