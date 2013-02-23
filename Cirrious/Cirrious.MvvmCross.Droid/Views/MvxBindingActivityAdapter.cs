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
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxBindingActivityAdapter
        : MvxBaseActivityAdapter
    {
        private IMvxBindingContext DroidBindingContext
        {
            get { return ((IMvxBindingContextOwner) Activity).BindingContext; }
        }

        public MvxBindingActivityAdapter(IMvxEventSourceActivity eventSource)
            : base(eventSource)
        {
        }

        protected override void EventSourceOnCreateWillBeCalled(object sender,
                                                                MvxValueEventArgs<Bundle> MvxValueEventArgs)
        {
            DroidBindingContext.ClearAllBindings();
            base.EventSourceOnCreateWillBeCalled(sender, MvxValueEventArgs);
        }

        protected override void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
            DroidBindingContext.ClearAllBindings();
            base.EventSourceOnDestroyCalled(sender, eventArgs);
        }

        protected override void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
            DroidBindingContext.ClearAllBindings();
            base.EventSourceOnDisposeCalled(sender, eventArgs);
        }
    }
}