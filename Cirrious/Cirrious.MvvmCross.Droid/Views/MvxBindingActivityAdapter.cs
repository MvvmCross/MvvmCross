using System;
using Android.OS;
using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Droid.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxBindingActivityAdapter
        : MvxBaseActivityAdapter
    {
        private IMvxBindingActivity BindingActivity
        {
            get { return (IMvxBindingActivity)Activity; }
        }

        public MvxBindingActivityAdapter(IMvxActivityEventSource eventSource) 
            : base(eventSource)
        {
        }

        protected override void EventSourceOnCreateWillBeCalled(object sender, MvxTypedEventArgs<Bundle> mvxTypedEventArgs)
        {
            BindingActivity.ClearAllBindings();
            base.EventSourceOnCreateWillBeCalled(sender, mvxTypedEventArgs);
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