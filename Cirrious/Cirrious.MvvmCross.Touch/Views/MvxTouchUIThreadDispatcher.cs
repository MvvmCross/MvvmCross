// MvxTouchUIThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using System.Threading;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views
{
    public abstract class MvxTouchUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        public bool RequestMainThreadAction(Action action)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    try
                    {
                        action();
                    }
                    catch (ThreadAbortException)
                    {
                        throw;
                    }
                    catch (TargetInvocationException exception)
                    {
                        MvxTrace.Trace("TargetInvocateException masked " + exception.InnerException.ToLongString());
                    }
                    catch (Exception exception)
                    {
                        // note - all exceptions masked!
                        MvxTrace.Warning("Exception masked " + exception.ToLongString());
                    }
                });
            return true;
        }
    }
}