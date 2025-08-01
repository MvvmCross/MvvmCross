// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Base;

namespace MvvmCross.WeakSubscription
{
#nullable enable
    public class MvxValueEventSubscription<TEventArgs>
        : MvxWeakEventSubscription<object, MvxValueEventArgs<TEventArgs>>
    {
        public MvxValueEventSubscription(object source,
                                         EventInfo eventInfo,
                                         EventHandler<MvxValueEventArgs<TEventArgs>> eventHandler)
            : base(source, eventInfo, eventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler<MvxValueEventArgs<TEventArgs>>(OnSourceEvent);
        }
    }
#nullable restore
}
