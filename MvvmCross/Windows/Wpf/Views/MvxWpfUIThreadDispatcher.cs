// MvxWpfUIThreadDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Wpf.Views
{
    using System;
    using System.Windows.Threading;

    using MvvmCross.Platform.Core;

    public class MvxWpfUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly Dispatcher _dispatcher;

        public MvxWpfUIThreadDispatcher(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        public bool RequestMainThreadAction(Action action)
        {
            if (this._dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                this._dispatcher.Invoke(() => ExceptionMaskedAction(action));
            }

            // TODO - why return bool at all?
            return true;
        }
    }
}