// MvxBindingFragmentAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Views;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;

namespace Cirrious.MvvmCross.Droid.Fragging
{
    public class MvxBindingFragmentAdapter
        : MvxBaseFragmentAdapter
    {
        protected IMvxAndroidFragmentView FragmentView
        {
            get { return base.Fragment as IMvxAndroidFragmentView; }
        }

        public MvxBindingFragmentAdapter(IMvxEventSourceFragment eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxAndroidFragmentView))
                throw new ArgumentException("eventSource must be an IMvxAndroidFragmentView");
        }

        private class MvxSimpleLayoutInflater : IMvxLayoutInflater
        {
            public MvxSimpleLayoutInflater(LayoutInflater layoutInflater)
            {
                LayoutInflater = layoutInflater;
            }

            public LayoutInflater LayoutInflater { get; private set; }
        }

        protected override void HandleCreateViewCalled(object sender,
                                                       MvxValueEventArgs<MvxCreateViewParameters> args)
        {
            if (FragmentView.BindingContext == null)
            {
                FragmentView.BindingContext = new MvxBindingContext(Fragment.Activity,
                                                                    new MvxSimpleLayoutInflater(args.Value.Inflater));
            }
        }

        protected override void HandleDestroyViewCalled(object sender, EventArgs eventArgs)
        {
            if (FragmentView.BindingContext != null)
            {
                FragmentView.BindingContext.ClearAllBindings();
            }
            base.HandleDestroyViewCalled(sender, eventArgs);
        }
    }
}