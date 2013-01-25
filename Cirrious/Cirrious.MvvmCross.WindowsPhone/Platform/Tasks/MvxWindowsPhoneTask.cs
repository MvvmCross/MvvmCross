// MvxWindowsPhoneTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.WindowsPhone.Platform.Tasks
{
    public class MvxWindowsPhoneTask 
		: IMvxServiceConsumer
    {
        protected IMvxViewDispatcher ViewDispatcher
        {
			get { return this.GetService<IMvxViewDispatcherProvider>().Dispatcher; }
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