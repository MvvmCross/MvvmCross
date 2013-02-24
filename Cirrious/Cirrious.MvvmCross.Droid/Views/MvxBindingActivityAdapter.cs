// MvxBindingActivityAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.OS;
using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxBindingActivityAdapter
        : MvxBaseActivityAdapter
    {
        private IMvxBindingContext BindingContext
        {
            get
            {
                var contextOwner = (IMvxBindingContextOwner) Activity;
                return (IMvxBindingContext) contextOwner.BindingContext;
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