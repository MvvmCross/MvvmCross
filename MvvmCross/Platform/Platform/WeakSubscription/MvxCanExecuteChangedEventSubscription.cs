// MvxCanExecuteChangedEventSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.WeakSubscription
{
    using System;
    using System.Reflection;
    using System.Windows.Input;

    public class MvxCanExecuteChangedEventSubscription
        : MvxWeakEventSubscription<ICommand, EventArgs>
    {
        private static readonly EventInfo CanExecuteChangedEventInfo = typeof(ICommand).GetEvent("CanExecuteChanged");

        public MvxCanExecuteChangedEventSubscription(ICommand source,
                                                    EventHandler<EventArgs> eventHandler)
            : base(source, CanExecuteChangedEventInfo, eventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler(this.OnSourceEvent);
        }
    }
}