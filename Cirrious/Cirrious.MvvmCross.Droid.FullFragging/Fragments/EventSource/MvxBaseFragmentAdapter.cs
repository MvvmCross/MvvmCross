// MvxBaseFragmentAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Cirrious.CrossCore.Core;

namespace Cirrious.MvvmCross.Droid.FullFragging.Fragments.EventSource
{
    public class MvxBaseFragmentAdapter
    {
        private readonly IMvxEventSourceFragment _eventSource;

        protected Fragment Fragment
        {
            get { return _eventSource as Fragment; }
        }

        public MvxBaseFragmentAdapter(IMvxEventSourceFragment eventSource)
        {
            if (eventSource == null)
                throw new ArgumentException("eventSource - eventSource should not be null");

            if (!(eventSource is Fragment))
                throw new ArgumentException("eventSource - eventSource should be a Fragment");

            _eventSource = eventSource;
            _eventSource.DisposeCalled += HandleDisposeCalled;
            _eventSource.OnCreateViewCalled += HandleCreateViewCalled;
            _eventSource.OnDestroyViewCalled += HandleDestroyViewCalled;
            _eventSource.OnAttachCalled += HandleAttachCalled;
        }

        protected virtual void HandleAttachCalled(object sender, MvxValueEventArgs<Activity> e)
        {
        }

        protected virtual void HandleDisposeCalled(object sender, EventArgs e)
        {
        }

        protected virtual void HandleDestroyViewCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void HandleCreateViewCalled(object sender,
                                                      MvxValueEventArgs<MvxCreateViewParameters> mvxValueEventArgs)
        {
        }
    }
}