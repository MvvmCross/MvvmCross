using System;
using Android.OS;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Droid.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxBindingActivityAdapter
        : BaseActivityAdapter
    {
        private IMvxBindingActivity BindingActivity
        {
            get { return (IMvxBindingActivity)Activity; }
        }

        public MvxBindingActivityAdapter(IActivityEventSource eventSource) 
            : base(eventSource)
        {
        }

        protected override void EventSourceOnCreateWillBeCalled(object sender, TypedEventArgs<Bundle> typedEventArgs)
        {
            BindingActivity.ClearAllBindings();
            base.EventSourceOnCreateWillBeCalled(sender, typedEventArgs);
        }

        protected override void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
            BindingActivity.ClearAllBindings();
            base.EventSourceOnDestroyCalled(sender, eventArgs);
        }

        protected override void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
            BindingActivity.ClearAllBindings();
            base.EventSourceOnDisposeCalled(sender, eventArgs);
        }
    }
}