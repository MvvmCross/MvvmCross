// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.OS;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Droid.Views;

namespace MvvmCross.Droid.Views
{
    public class MvxBindingActivityAdapter
        : MvxBaseActivityAdapter
    {
        private IMvxAndroidBindingContext BindingContext
        {
            get
            {
                var contextOwner = (IMvxBindingContextOwner)Activity;
                return (IMvxAndroidBindingContext)contextOwner.BindingContext;
            }
        }

        public MvxBindingActivityAdapter(IMvxEventSourceActivity eventSource)
            : base(eventSource)
        {
        }

        protected override void EventSourceOnCreateWillBeCalled(object sender,
                                                                MvxValueEventArgs<Bundle> MvxValueEventArgs)
        {
            BindingContext.ClearAllBindings();
            base.EventSourceOnCreateWillBeCalled(sender, MvxValueEventArgs);
        }

        protected override void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
            BindingContext.ClearAllBindings();
            base.EventSourceOnDestroyCalled(sender, eventArgs);
        }

        protected override void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
            BindingContext.ClearAllBindings();
            base.EventSourceOnDisposeCalled(sender, eventArgs);
        }
    }
}