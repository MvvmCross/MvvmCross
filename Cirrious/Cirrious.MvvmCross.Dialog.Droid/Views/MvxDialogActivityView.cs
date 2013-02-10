// MvxDialogActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;
using CrossUI.Droid.Dialog;

namespace Cirrious.MvvmCross.Dialog.Droid.Views
{
    public abstract class EventSourceDialogActivity
        : DialogActivity
        , IActivityEventSource
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

        public override void StartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResultCalled.Raise(this, new StartActivityForResultParameters(intent, requestCode));
            base.StartActivityForResult(intent, requestCode);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            ActivityResultCalled.Raise(this, new ActivityResultParameters(requestCode, resultCode, data));
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public event EventHandler DisposeCalled;
        public event EventHandler<TypedEventArgs<Bundle>> CreateWillBeCalled;
        public event EventHandler<TypedEventArgs<Bundle>> CreateCalled;
        public event EventHandler DestroyCalled;
        public event EventHandler<TypedEventArgs<Intent>> NewIntentCalled;
        public event EventHandler ResumeCalled;
        public event EventHandler PauseCalled;
        public event EventHandler StartCalled;
        public event EventHandler RestartCalled;
        public event EventHandler StopCalled;
        public event EventHandler<TypedEventArgs<StartActivityForResultParameters>> StartActivityForResultCalled;
        public event EventHandler<TypedEventArgs<ActivityResultParameters>> ActivityResultCalled;
    }

    public abstract class MvxDialogActivityView
        : EventSourceDialogActivity
        , IMvxAndroidView
    {
        protected MvxDialogActivityView()
        {
            BindingOwnerHelper = new MvxBindingOwnerHelper(this, this, this);
            this.AddEventListeners();
        }

        public bool IsVisible { get; set; }

        public object DataContext { get; set; }

        public IMvxViewModel ViewModel
        {
            get { return (IMvxViewModel)DataContext; }
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            base.StartActivityForResult(intent, requestCode);
        }

        protected abstract void OnViewModelSet();

        public IMvxBindingOwnerHelper BindingOwnerHelper { get; private set; }

        public override void SetContentView(int layoutResId)
        {
            var view = this.BindingInflate(layoutResId, null);
            SetContentView(view);
        }
    }
}