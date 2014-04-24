// MvxChildViewModelOwnerAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.CrossCore.Exceptions;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxChildViewModelOwnerAdapterF : MvxBaseFragmentAdapter
    {
        protected IMvxChildViewModelOwner ChildOwner
        {
            get { return (IMvxChildViewModelOwner) base.Fragment; }
        }

        public MvxChildViewModelOwnerAdapterF(IMvxEventSourceFragment eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxChildViewModelOwner))
            {
                throw new MvxException("You cannot use a MvxChildViewModelOwnerAdapterF on {0}",
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