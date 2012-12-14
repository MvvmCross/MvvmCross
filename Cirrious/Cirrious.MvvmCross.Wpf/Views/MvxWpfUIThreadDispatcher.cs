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
#warning Should we mask all these exceptions?
                MvxTrace.Trace("Exception masked " + exception.ToLongString());
            }
        }
    }
}