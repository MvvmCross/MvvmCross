#region Copyright

// <copyright file="MvxWpfUIThreadDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Reflection;
using System.Threading;
using System.Windows.Threading;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Wpf.Views
{
    public class MvxWpfUIThreadDispatcher
        : IMvxMainThreadDispatcher
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
                MvxTrace.Trace("Exception masked " + exception.ToLongString());
            }
        }
    }
}