// MvxEventSourceListFragment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Droid.Views;

namespace Cirrious.MvvmCross.Droid.Fragging.Fragments.EventSource
{
    public class MvxEventSourceListFragment
        : ListFragment
          , IMvxEventSourceFragment
    {
        public event EventHandler DisposeCalled;
        public event EventHandler<MvxValueEventArgs<MvxCreateViewParameters>> OnCreateViewCalled;
        public event EventHandler OnDestroyViewCalled;
        public event EventHandler<MvxValueEventArgs<Activity>> OnAttachCalled;

        public override void OnAttach(Activity activity)
        {
            OnAttachCalled.Raise(this, activity);
            base.OnAttach(activity);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            OnCreateViewCalled.Raise(this, new MvxCreateViewParameters(inflater, container, savedInstanceState));
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnDestroyView()
        {
            OnDestroyViewCalled.Raise(this);
            base.OnDestroyView();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }
    }
}