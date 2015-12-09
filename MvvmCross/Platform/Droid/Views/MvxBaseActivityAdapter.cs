// MvxBaseActivityAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Droid.Views
{
    using System;

    using Android.App;
    using Android.Content;
    using Android.OS;

    using MvvmCross.Platform.Core;

    public class MvxBaseActivityAdapter
    {
        private readonly IMvxEventSourceActivity _eventSource;

        protected Activity Activity => this._eventSource as Activity;

        protected MvxBaseActivityAdapter(IMvxEventSourceActivity eventSource)
        {
            this._eventSource = eventSource;

            this._eventSource.CreateCalled += this.EventSourceOnCreateCalled;
            this._eventSource.CreateWillBeCalled += this.EventSourceOnCreateWillBeCalled;
            this._eventSource.StartCalled += this.EventSourceOnStartCalled;
            this._eventSource.RestartCalled += this.EventSourceOnRestartCalled;
            this._eventSource.ResumeCalled += this.EventSourceOnResumeCalled;
            this._eventSource.PauseCalled += this.EventSourceOnPauseCalled;
            this._eventSource.StopCalled += this.EventSourceOnStopCalled;
            this._eventSource.DestroyCalled += this.EventSourceOnDestroyCalled;
            this._eventSource.DisposeCalled += this.EventSourceOnDisposeCalled;
            this._eventSource.SaveInstanceStateCalled += this.EventSourceOnSaveInstanceStateCalled;
            this._eventSource.NewIntentCalled += this.EventSourceOnNewIntentCalled;

            this._eventSource.ActivityResultCalled += this.EventSourceOnActivityResultCalled;
            this._eventSource.StartActivityForResultCalled += this.EventSourceOnStartActivityForResultCalled;
        }

        protected virtual void EventSourceOnSaveInstanceStateCalled(object sender,
                                                                    MvxValueEventArgs<Bundle> mvxValueEventArgs)
        {
        }

        protected virtual void EventSourceOnCreateWillBeCalled(object sender,
                                                               MvxValueEventArgs<Bundle> MvxValueEventArgs)
        {
        }

        protected virtual void EventSourceOnStopCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnStartCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnStartActivityForResultCalled(object sender,
                                                                         MvxValueEventArgs
                                                                             <MvxStartActivityForResultParameters>
                                                                             MvxValueEventArgs)
        {
        }

        protected virtual void EventSourceOnResumeCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnRestartCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnPauseCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnNewIntentCalled(object sender, MvxValueEventArgs<Intent> MvxValueEventArgs)
        {
        }

        protected virtual void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnCreateCalled(object sender, MvxValueEventArgs<Bundle> MvxValueEventArgs)
        {
        }

        protected virtual void EventSourceOnActivityResultCalled(object sender,
                                                                 MvxValueEventArgs<MvxActivityResultParameters>
                                                                     MvxValueEventArgs)
        {
        }
    }
}