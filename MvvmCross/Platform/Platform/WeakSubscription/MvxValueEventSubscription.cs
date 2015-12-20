// MvxValueEventSubscription.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.WeakSubscription
{
    using System;
    using System.Reflection;

    using MvvmCross.Platform.Core;

    public class MvxValueEventSubscription<T>
        : MvxWeakEventSubscription<object, MvxValueEventArgs<T>>
    {
        public MvxValueEventSubscription(object source,
                                         EventInfo eventInfo,
                                         EventHandler<MvxValueEventArgs<T>> eventHandler)
            : base(source, eventInfo, eventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler<MvxValueEventArgs<T>>(this.OnSourceEvent);
        }
    }
}