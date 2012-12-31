#region Copyright

// <copyright file="MvxWindowsPhoneTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.WindowsPhone.Platform.Tasks
{
    public class MvxWindowsPhoneTask : IMvxServiceConsumer<IMvxViewDispatcherProvider>
    {
        protected IMvxViewDispatcher ViewDispatcher
        {
            get { return this.GetService().Dispatcher; }
        }

        protected void DoWithInvalidOperationProtection(Action action)
        {
            ViewDispatcher.RequestMainThreadAction(() =>
                {
                    try
                    {
                        action();
                    }
                    catch (InvalidOperationException exception)
                    {
                        // note - all exceptions masked here
                        MvxTrace.Trace("Exception masked in {0} - error was {1}", this, exception.ToLongString());
                    }
                });
        }
    }
}