using System;
using Cirrious.MvvmCross.Exceptions;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxChildViewModelOwnerAdapter : BaseActivityAdapter
    {
        protected IMvxChildViewModelOwner  ChildOwner
        {
            get { return (IMvxChildViewModelOwner)base.Activity; }
        }

        public MvxChildViewModelOwnerAdapter(IActivityEventSource eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxChildViewModelOwner))
            {
                throw new MvxException("You cannot use a MvxChildViewModelOwnerAdapter on {0}", eventSource.GetType().Name);
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