// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Android.Content;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.Hosting;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views.Base;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Views
{
    [RequiresUnreferencedCode("Loading ViewModels requires unreferenced code")]
    public class MvxActivityAdapter : MvxBaseActivityAdapter
    {
        protected IMvxAndroidView? AndroidView => Activity as IMvxAndroidView;

        public MvxActivityAdapter(IMvxEventSourceActivity eventSource)
            : base(eventSource)
        {
        }

        protected override void EventSourceOnStopCalled(object? sender, EventArgs eventArgs)
        {
            AndroidView?.OnViewStop();
        }

        protected override void EventSourceOnStartCalled(object? sender, EventArgs eventArgs)
        {
            AndroidView?.OnViewStart();
        }

        protected override void EventSourceOnStartActivityForResultCalled(
            object? sender, MvxValueEventArgs<MvxStartActivityForResultParameters> eventArgs)
        {
            var requestCode = eventArgs.Value.RequestCode;
            switch (requestCode)
            {
                case (int)MvxIntentRequestCode.PickFromFile:
                    MvxLogHost.GetLog<MvxActivityAdapter>()?.Log(LogLevel.Warning,
                        "Warning - activity request code may clash with Mvx code for {requestCode}",
                        (MvxIntentRequestCode)requestCode);
                    break;
            }
        }

        protected override void EventSourceOnResumeCalled(object? sender, EventArgs eventArgs)
        {
            AndroidView?.OnViewResume();
        }

        protected override void EventSourceOnRestartCalled(object? sender, EventArgs eventArgs)
        {
            AndroidView?.OnViewRestart();
        }

        protected override void EventSourceOnPauseCalled(object? sender, EventArgs eventArgs)
        {
            AndroidView?.OnViewPause();
        }

        protected override void EventSourceOnNewIntentCalled(object? sender, MvxValueEventArgs<Intent?> eventArgs)
        {
            AndroidView?.OnViewNewIntent();
        }

        protected override void EventSourceOnDestroyCalled(object? sender, EventArgs eventArgs)
        {
            AndroidView?.OnViewDestroy();
        }

        protected override void EventSourceOnCreateCalled(object? sender, MvxValueEventArgs<Bundle?> eventArgs)
        {
            AndroidView?.OnViewCreate(eventArgs.Value!);
        }

        protected override void EventSourceOnSaveInstanceStateCalled(object? sender, MvxValueEventArgs<Bundle> eventArgs)
        {
            var mvxBundle = AndroidView?.CreateSaveStateBundle();
            if (mvxBundle != null)
            {
                var converter = MvxHost.Current?.Services.GetService<IMvxSavedStateConverter>();
                if (converter == null)
                {
                    MvxLogHost.GetLog<MvxActivityAdapter>()?.Log(LogLevel.Warning,
                        "Saved state converter not available - saving state will be hard");
                }
                else
                {
                    converter.Write(eventArgs.Value, mvxBundle);
                }
            }

            var cache = MvxHost.Current?.Services.GetService<IMvxSingleViewModelCache>();
            if (cache != null)
            {
                cache.Cache(AndroidView?.ViewModel!, eventArgs.Value);
            }
        }

        protected override void EventSourceOnActivityResultCalled(
            object? sender, MvxValueEventArgs<MvxActivityResultParameters> eventArgs)
        {
            var sink = MvxHost.Current?.Services.GetService<IMvxIntentResultSink>();
            if (sink != null)
            {
                var resultParameters = eventArgs.Value;
                var intentResult = new MvxIntentResultEventArgs(
                    resultParameters.RequestCode,
                    resultParameters.ResultCode,
                    resultParameters.Data!);
                sink.OnResult(intentResult);
            }
        }
    }
}
