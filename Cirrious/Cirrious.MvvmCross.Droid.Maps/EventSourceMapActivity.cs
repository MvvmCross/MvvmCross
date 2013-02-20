using System;
using Android.App;
using Android.Content;
using Android.GoogleMaps;
using Android.OS;
using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Droid.Views;

namespace Cirrious.MvvmCross.Droid.Maps
{
    public abstract class EventSourceMapActivity
        : MapActivity
          , IMvxActivityEventSource
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
        }

        public override void OnNewIntent(Intent intent)
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

        public event EventHandler DisposeCalled;
        public event EventHandler<MvxTypedEventArgs<Bundle>> CreateWillBeCalled;
        public event EventHandler<MvxTypedEventArgs<Bundle>> CreateCalled;
        public event EventHandler DestroyCalled;
        public event EventHandler<MvxTypedEventArgs<Intent>> NewIntentCalled;
        public event EventHandler ResumeCalled;
        public event EventHandler PauseCalled;
        public event EventHandler StartCalled;
        public event EventHandler RestartCalled;
        public event EventHandler StopCalled;
        public event EventHandler<MvxTypedEventArgs<MvxStartActivityForResultParameters>> StartActivityForResultCalled;
        public event EventHandler<MvxTypedEventArgs<MvxActivityResultParameters>> ActivityResultCalled;
    }
}