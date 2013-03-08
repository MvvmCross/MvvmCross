// MvxActivityAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using Android.Content;
using Android.OS;
using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxActivityAdapter : MvxBaseActivityAdapter
    {
        protected IMvxAndroidView AndroidView
        {
            get { return Activity as IMvxAndroidView; }
        }

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
                                                                          MvxValueEventArgs
                                                                              <MvxStartActivityForResultParameters>
                                                                              MvxValueEventArgs)
        {
            var requestCode = MvxValueEventArgs.Value.RequestCode;
            switch (requestCode)
            {
                case (int) MvxIntentRequestCode.PickFromFile:
                    MvxTrace.Trace("Warning - activity request code may clash with Mvx code for {0}",
                                   (MvxIntentRequestCode) requestCode);
                    break;
            }
        }

        protected override void EventSourceOnResumeCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.IsVisible = true;
            AndroidView.OnViewResume();
        }

        protected override void EventSourceOnRestartCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewRestart();
        }

        protected override void EventSourceOnPauseCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewPause();
            AndroidView.IsVisible = false;
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
            AndroidView.IsVisible = true;
            AndroidView.OnViewCreate(eventArgs.Value);
        }

        protected override void EventSourceOnSaveInstanceStateCalled(object sender, MvxValueEventArgs<Bundle> bundleArgs)
        {
            var converter = Mvx.Resolve<IMvxSavedStateConverter>();
            var mvxBundle = GetSaveStateBundle();
            if (mvxBundle != null)
            {
                converter.Write(bundleArgs.Value, mvxBundle);
            }
        }

        private IMvxBundle GetSaveStateBundle()
        {
            var toReturn = new MvxBundle();
            var androidView = AndroidView;
            var viewModel = androidView.ViewModel;
            if (viewModel == null)
                return toReturn;

            var method = viewModel.GetType().GetMethods().FirstOrDefault(m => m.Name == "SaveState");

            if (method.GetParameters().Any() ||
                method.ReturnType == typeof (void))
            {
                viewModel.SaveState(toReturn);
                return toReturn;
            }
            var stateObject = method.Invoke(viewModel, new object[0]);
            if (stateObject != null)
            {
                toReturn.Write(stateObject);
            }
            return toReturn;
        }

        protected override void EventSourceOnActivityResultCalled(object sender,
                                                                  MvxValueEventArgs<MvxActivityResultParameters>
                                                                      MvxValueEventArgs)
        {
            var sink = Mvx.Resolve<IMvxIntentResultSink>();
            var args = MvxValueEventArgs.Value;
            var intentResult = new MvxIntentResultEventArgs(args.RequestCode, args.ResultCode, args.Data);
            sink.OnResult(intentResult);
        }
    }
}