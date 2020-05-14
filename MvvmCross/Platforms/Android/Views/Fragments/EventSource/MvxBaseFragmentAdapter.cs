﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.OS;
using AndroidX.Fragment.App;
using MvvmCross.Base;

namespace MvvmCross.Platforms.Android.Views.Fragments.EventSource
{
    public class MvxBaseFragmentAdapter
    {
        private readonly IMvxEventSourceFragment _eventSource;

        protected Fragment Fragment => _eventSource as Fragment;

        public MvxBaseFragmentAdapter(IMvxEventSourceFragment eventSource)
        {
            if (eventSource is null)
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

        protected virtual void HandleAttachCalled(object sender, MvxValueEventArgs<Context> e)
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
