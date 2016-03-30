// MvxBaseFragmentAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.OS;
using MvvmCross.Platform.Core;
using System;
using Fragment = Android.Support.V4.App.Fragment;
using MvvmCross.Droid.Shared.Fragments.EventSource;
using MvvmCross.Droid.Shared;

namespace MvvmCross.Droid.Support.V7.Fragging.Fragments.EventSource
{
    public class MvxBaseFragmentAdapter
    {
        private readonly IMvxEventSourceFragment _eventSource;

        protected Fragment Fragment => _eventSource as Fragment;

        public MvxBaseFragmentAdapter(IMvxEventSourceFragment eventSource)
        {
            if (eventSource == null)
                throw new ArgumentException("eventSource should not be null", nameof(eventSource));

            if (!(eventSource is Fragment))
                throw new ArgumentException("eventSource should be a Fragment", nameof(eventSource));

            _eventSource = eventSource;
            _eventSource.DisposeCalled += HandleDisposeCalled;
            _eventSource.CreateViewCalled += HandleCreateViewCalled;
            _eventSource.DestroyViewCalled += HandleDestroyViewCalled;
            _eventSource.AttachCalled += HandleAttachCalled;
            _eventSource.CreateCalled += HandleCreateCalled;
            _eventSource.StartCalled += HandleStartCalled;
            _eventSource.StopCalled += HandleStopCalled;
            _eventSource.PauseCalled += HandlePauseCalled;
            _eventSource.ResumeCalled += HandleResumeCalled;
            _eventSource.DetachCalled += HandleDetachCalled;
            _eventSource.SaveInstanceStateCalled += HandleSaveInstanceStateCalled;
        }

        protected virtual void HandleSaveInstanceStateCalled(object sender, MvxValueEventArgs<Bundle> e)
        {
        }

        protected virtual void HandleDetachCalled(object sender, EventArgs e)
        {
        }

        protected virtual void HandleResumeCalled(object sender, EventArgs e)
        {
        }

        protected virtual void HandlePauseCalled(object sender, EventArgs e)
        {
        }

        protected virtual void HandleStopCalled(object sender, EventArgs e)
        {
        }

        protected virtual void HandleStartCalled(object sender, EventArgs e)
        {
        }

        protected virtual void HandleCreateCalled(object sender, MvxValueEventArgs<Bundle> e)
        {
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