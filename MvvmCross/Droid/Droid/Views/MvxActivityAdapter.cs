// MvxActivityAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Views
{
    using System;

    using Android.Content;
    using Android.OS;

    using MvvmCross.Core.Views;
    using MvvmCross.Droid.Platform;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Droid.Platform;
    using MvvmCross.Platform.Droid.Views;
    using MvvmCross.Platform.Platform;

    public class MvxActivityAdapter : MvxBaseActivityAdapter
    {
        protected IMvxAndroidView AndroidView => Activity as IMvxAndroidView;

        public MvxActivityAdapter(IMvxEventSourceActivity eventSource)
            : base(eventSource)
        {
        }

        protected override void EventSourceOnStopCalled(object sender, EventArgs eventArgs)
        {
            this.AndroidView.OnViewStop();
        }

        protected override void EventSourceOnStartCalled(object sender, EventArgs eventArgs)
        {
            this.AndroidView.OnViewStart();
        }

        protected override void EventSourceOnStartActivityForResultCalled(object sender,
                                                                          MvxValueEventArgs
                                                                              <MvxStartActivityForResultParameters>
                                                                              MvxValueEventArgs)
        {
            var requestCode = MvxValueEventArgs.Value.RequestCode;
            switch (requestCode)
            {
                case (int)MvxIntentRequestCode.PickFromFile:
                    MvxTrace.Warning("Warning - activity request code may clash with Mvx code for {0}",
                                   (MvxIntentRequestCode)requestCode);
                    break;
            }
        }

        protected override void EventSourceOnResumeCalled(object sender, EventArgs eventArgs)
        {
            this.AndroidView.OnViewResume();
        }

        protected override void EventSourceOnRestartCalled(object sender, EventArgs eventArgs)
        {
            this.AndroidView.OnViewRestart();
        }

        protected override void EventSourceOnPauseCalled(object sender, EventArgs eventArgs)
        {
            this.AndroidView.OnViewPause();
        }

        protected override void EventSourceOnNewIntentCalled(object sender, MvxValueEventArgs<Intent> MvxValueEventArgs)
        {
            this.AndroidView.OnViewNewIntent();
        }

        protected override void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
            this.AndroidView.OnViewDestroy();
        }

        protected override void EventSourceOnCreateCalled(object sender, MvxValueEventArgs<Bundle> eventArgs)
        {
            this.AndroidView.OnViewCreate(eventArgs.Value);
        }

        protected override void EventSourceOnSaveInstanceStateCalled(object sender, MvxValueEventArgs<Bundle> bundleArgs)
        {
            var mvxBundle = this.AndroidView.CreateSaveStateBundle();
            if (mvxBundle != null)
            {
                IMvxSavedStateConverter converter;
                if (!Mvx.TryResolve<IMvxSavedStateConverter>(out converter))
                {
                    MvxTrace.Warning("Saved state converter not available - saving state will be hard");
                }
                else
                {
                    converter.Write(bundleArgs.Value, mvxBundle);
                }
            }
            var cache = Mvx.Resolve<IMvxSingleViewModelCache>();
            cache.Cache(this.AndroidView.ViewModel, bundleArgs.Value);
        }

        protected override void EventSourceOnActivityResultCalled(object sender,
                                                                  MvxValueEventArgs<MvxActivityResultParameters>
                                                                      args)
        {
            var sink = Mvx.Resolve<IMvxIntentResultSink>();
            var resultParameters = args.Value;
            var intentResult = new MvxIntentResultEventArgs(resultParameters.RequestCode, resultParameters.ResultCode,
                                                            resultParameters.Data);
            sink.OnResult(intentResult);
        }
    }
}