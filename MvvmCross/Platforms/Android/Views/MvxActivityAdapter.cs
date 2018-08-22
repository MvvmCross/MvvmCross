// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.OS;
using MvvmCross.Base;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Views.Base;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxActivityAdapter : MvxBaseActivityAdapter
    {
        protected IMvxAndroidView AndroidView => Activity as IMvxAndroidView;

        public MvxActivityAdapter(IMvxEventSourceActivity eventSource)
            : base(eventSource)
        {
        }

        protected override void EventSourceOnStopCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewStop();
        }

        protected override void EventSourceOnStartCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewStart();
        }

        protected override void EventSourceOnStartActivityForResultCalled(object sender, 
                                                                          MvxValueEventArgs<MvxStartActivityForResultParameters> MvxValueEventArgs)
        {
            var requestCode = MvxValueEventArgs.Value.RequestCode;
            switch (requestCode)
            {
                case (int)MvxIntentRequestCode.PickFromFile:
                    MvxLog.Instance.Warn("Warning - activity request code may clash with Mvx code for {0}",
                                   (MvxIntentRequestCode)requestCode);
                    break;
            }
        }

        protected override void EventSourceOnResumeCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewResume();
        }

        protected override void EventSourceOnRestartCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewRestart();
        }

        protected override void EventSourceOnPauseCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewPause();
        }

        protected override void EventSourceOnNewIntentCalled(object sender, MvxValueEventArgs<Intent> MvxValueEventArgs)
        {
            AndroidView.OnViewNewIntent();
        }

        protected override void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewDestroy();
        }

        protected override void EventSourceOnCreateCalled(object sender, MvxValueEventArgs<Bundle> eventArgs)
        {
            AndroidView.OnViewCreate(eventArgs.Value);
        }

        protected override void EventSourceOnSaveInstanceStateCalled(object sender, MvxValueEventArgs<Bundle> bundleArgs)
        {
            var mvxBundle = AndroidView.CreateSaveStateBundle();
            if (mvxBundle != null)
            {
                IMvxSavedStateConverter converter;
                if (!Mvx.IoCProvider.TryResolve<IMvxSavedStateConverter>(out converter))
                {
                    MvxLog.Instance.Warn("Saved state converter not available - saving state will be hard");
                }
                else
                {
                    converter.Write(bundleArgs.Value, mvxBundle);
                }
            }
            var cache = Mvx.IoCProvider.Resolve<IMvxSingleViewModelCache>();
            cache.Cache(AndroidView.ViewModel, bundleArgs.Value);
        }

        protected override void EventSourceOnActivityResultCalled(object sender,
                                                                  MvxValueEventArgs<MvxActivityResultParameters> args)
        {
            var sink = Mvx.IoCProvider.Resolve<IMvxIntentResultSink>();
            var resultParameters = args.Value;
            var intentResult = new MvxIntentResultEventArgs(resultParameters.RequestCode, resultParameters.ResultCode,
                                                            resultParameters.Data);
            sink.OnResult(intentResult);
        }
    }
}
