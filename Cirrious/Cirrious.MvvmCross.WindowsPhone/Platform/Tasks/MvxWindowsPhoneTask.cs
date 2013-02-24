// MvxWindowsPhoneTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.WindowsPhone.Platform.Tasks
{
    public class MvxWindowsPhoneTask
        : IMvxConsumer
    {
        protected IMvxViewDispatcher ViewDispatcher
        {
            get { return this.Resolve<IMvxViewDispatcherProvider>().ViewDispatcher; }
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