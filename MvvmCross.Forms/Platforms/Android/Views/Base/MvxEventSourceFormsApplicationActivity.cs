// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.App;
using Android.Content;
using Android.OS;
using MvvmCross.Base;
using MvvmCross.Platforms.Android.Views.Base;
using MvvmCross.ViewModels;
using Xamarin.Forms.Platform.Android;

namespace MvvmCross.Forms.Platforms.Android.Views.Base
{
    public abstract class MvxEventSourceFormsApplicationActivity
        : FormsApplicationActivity, IMvxEventSourceActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            CreateWillBeCalled.Raise(this, bundle);
            base.OnCreate(bundle);
            CreateCalled.Raise(this, bundle);
        }

        protected override void OnDestroy()
        {
            DestroyCalled.Raise(this);
            base.OnDestroy();

            if (IsTaskRoot)
            {
                var appStart = Mvx.IoCProvider.Resolve<IMvxAppStart>();
                appStart.ResetStart();
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            NewIntentCalled.Raise(this, intent);
        }

        protected override void OnResume()
        {
            base.OnResume();
            ResumeCalled.Raise(this);
        }

        protected override void OnPause()
        {
            PauseCalled.Raise(this);
            base.OnPause();
        }

        protected override void OnStart()
        {
            base.OnStart();
            StartCalled.Raise(this);
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            RestartCalled.Raise(this);
        }

        protected override void OnStop()
        {
            StopCalled.Raise(this);
            base.OnStop();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            SaveInstanceStateCalled.Raise(this, outState);
            base.OnSaveInstanceState(outState);
        }

        public override void StartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResultCalled.Raise(this, new MvxStartActivityForResultParameters(intent, requestCode));
            base.StartActivityForResult(intent, requestCode);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            ActivityResultCalled.Raise(this, new MvxActivityResultParameters(requestCode, resultCode, data));
            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }

        public event EventHandler DisposeCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> CreateWillBeCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> CreateCalled;

        public event EventHandler DestroyCalled;

        public event EventHandler<MvxValueEventArgs<Intent>> NewIntentCalled;

        public event EventHandler ResumeCalled;

        public event EventHandler PauseCalled;

        public event EventHandler StartCalled;

        public event EventHandler RestartCalled;

        public event EventHandler StopCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> SaveInstanceStateCalled;

        public event EventHandler<MvxValueEventArgs<MvxStartActivityForResultParameters>> StartActivityForResultCalled;

        public event EventHandler<MvxValueEventArgs<MvxActivityResultParameters>> ActivityResultCalled;
    }
}
