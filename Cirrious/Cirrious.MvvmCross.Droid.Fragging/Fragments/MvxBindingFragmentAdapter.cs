// MvxBindingFragmentAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments.EventSource;

namespace Cirrious.MvvmCross.Droid.Fragging.Fragments
{
    public class MvxBindingFragmentAdapter
        : MvxBaseFragmentAdapter
    {
        protected IMvxFragmentView FragmentView
        {
            get { return base.Fragment as IMvxFragmentView; }
        }

        public MvxBindingFragmentAdapter(IMvxEventSourceFragment eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxFragmentView))
                throw new ArgumentException("eventSource must be an IMvxFragmentView");
        }

        protected override void HandleCreateViewCalled(object sender,
                                                       MvxValueEventArgs<MvxCreateViewParameters> args)
        {
            FragmentView.EnsureBindingContextIsSet(args.Value.Inflater);
        }

        protected override void HandleDestroyViewCalled(object sender, EventArgs eventArgs)
        {
            if (FragmentView.BindingContext != null)
                {
                FragmentView.BindingContext.ClearAllBindings();
            }
            base.HandleDestroyViewCalled(sender, eventArgs);
        }

        protected override void HandleDisposeCalled(object sender, EventArgs e)
        {
            if (FragmentView.BindingContext != null)
            {
                FragmentView.BindingContext.ClearAllBindings();
            }
            base.HandleDisposeCalled(sender, e);
        }
    }
}