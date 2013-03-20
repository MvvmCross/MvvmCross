// MvxWpfUIThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using System.Threading;
using System.Windows.Threading;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Wpf.Views
{
    public class MvxWpfUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly Dispatcher _dispatcher;

        public MvxWpfUIThreadDispatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public bool RequestMainThreadAction(Action action)
        {
            if (_dispatcher.CheckAccess())
            {
                DoAction(action);
            }
            else
            {
                _dispatcher.Invoke(() => DoAction(action));
            }

            // TODO - why return bool at all?
            return true;
        }

        private static void DoAction(Action action)
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
                // note - all exceptions masked
                MvxTrace.Warning("Exception masked " + exception.ToLongString());
            }
        }
    }
}