#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXPhoneViewDispatcher.cs" company="Cirrious">
// //     (c) Copyright Cirrious. http://www.cirrious.com
// //     This source is subject to the Microsoft Public License (Ms-PL)
// //     Please see license.txt on http://opensource.org/licenses/ms-pl.html
// //     All other rights reserved.
// // </copyright>
// // 
// // Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// // ------------------------------------------------------------------------

#endregion

#region using

using System.Collections.Generic;
using System.Windows.Threading;
using Cirrious.MonoCross.Extensions.Interfaces;
using MonoCross.Navigation;

#endregion

namespace Cirrious.MonoCross.Extensions.WindowsPhone
{
    public class MXPhoneViewDispatcher : MxCrossThreadDispatcher, IMXViewDispatcher
    {
        public MXPhoneViewDispatcher(Dispatcher uiDispatcher)
            : base(uiDispatcher)
        {
        }

        public bool RequestNavigate(string url, Dictionary<string, string> parameters)
        {
            return InvokeOrBeginInvoke(() =>
                                           {
                                               // note that we are passing a null "fromView" into this method
                                               // this is a shame - as it looks ugly - but seemed unavoidable...
                                               // is this fromView parameter really needed?
                                               MXContainer.Navigate(null, url, parameters);
                                           });
        }
    }
}