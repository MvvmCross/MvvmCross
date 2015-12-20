// MvxBindingActivityAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Views
{
    using System;

    using Android.OS;

    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Droid.BindingContext;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Droid.Views;

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
            this.BindingContext.ClearAllBindings();
            base.EventSourceOnCreateWillBeCalled(sender, MvxValueEventArgs);
        }

        protected override void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
            this.BindingContext.ClearAllBindings();
            base.EventSourceOnDestroyCalled(sender, eventArgs);
        }

        protected override void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
            this.BindingContext.ClearAllBindings();
            base.EventSourceOnDisposeCalled(sender, eventArgs);
        }
    }
}