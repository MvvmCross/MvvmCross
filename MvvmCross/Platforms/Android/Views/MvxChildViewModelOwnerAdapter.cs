// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Exceptions;
using MvvmCross.Platforms.Android.Views.Base;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxChildViewModelOwnerAdapter : MvxBaseActivityAdapter
    {
        protected IMvxChildViewModelOwner ChildOwner => (IMvxChildViewModelOwner)Activity;

        public MvxChildViewModelOwnerAdapter(IMvxEventSourceActivity eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxChildViewModelOwner))
            {
                throw new MvxException("You cannot use a MvxChildViewModelOwnerAdapter on {0}",
                                       eventSource.GetType().Name);
            }
        }

        protected override void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
            ChildOwner.ClearOwnedSubIndicies();
            base.EventSourceOnDestroyCalled(sender, eventArgs);
        }

        protected override void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
            ChildOwner.ClearOwnedSubIndicies();
            base.EventSourceOnDisposeCalled(sender, eventArgs);
        }
    }
}
