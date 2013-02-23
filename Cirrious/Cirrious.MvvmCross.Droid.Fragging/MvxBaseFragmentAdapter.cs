// MvxBaseFragmentAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Support.V4.App;
using Cirrious.CrossCore.Interfaces.Core;

namespace Cirrious.MvvmCross.Droid.Fragging
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
        }

        public virtual void HandleDisposeCalled(object sender, EventArgs e)
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