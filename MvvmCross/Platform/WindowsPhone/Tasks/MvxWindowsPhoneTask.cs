// MvxWindowsPhoneTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.WindowsPhone.Tasks
{
    using System;

    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public class MvxWindowsPhoneTask
        : MvxMainThreadDispatchingObject
    {
        protected void DoWithInvalidOperationProtection(Action action)
        {
            this.Dispatcher.RequestMainThreadAction(() =>
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