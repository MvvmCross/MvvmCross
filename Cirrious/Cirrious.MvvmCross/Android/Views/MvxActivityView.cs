#region Copyright

// <copyright file="MvxActivityView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Cirrious.MvvmCross.Android.ExtensionMethods;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Android.Views
{
    public abstract class MvxActivityView<TViewModel>
        : Activity
        , IMvxAndroidView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected MvxActivityView()
        {
            IsVisible = true;
        }

        #region Common code across all android views - one case for multiple inheritance?

        public Type ViewModelType
        {
            get { return typeof(TViewModel); }
        }

        public bool IsVisible { get; private set; }

        private TViewModel _viewModel;

        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                OnViewModelSet();
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            this.OnViewCreate();
            base.OnCreate(bundle);
        }

        protected override void OnDestroy()
        {
            this.OnViewDestroy();
            base.OnDestroy();
        }


        protected abstract void OnViewModelSet();

        protected override void OnResume()
        {
            this.OnViewResume();
            IsVisible = true;
            base.OnResume();
        }

        protected override void OnPause()
        {
            this.OnViewPause();
            IsVisible = false;
            base.OnPause();
        }

        protected override void OnStart()
        {
            this.OnViewStart();
            base.OnStart();
        }

        protected override void OnRestart()
        {
            this.OnViewRestart();
            base.OnRestart();
        }

        protected override void OnStop()
        {
            this.OnViewStop();
            base.OnStop();
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            base.StartActivityForResult(intent, requestCode);
        }

        public event EventHandler<MvxIntentResultEventArgs> MvxIntentResultReceived;
 
        public override void StartActivityForResult(global::Android.Content.Intent intent, int requestCode)
        {
            switch (requestCode)
            {
                case (int)MvxIntentRequestCode.PickFromFile:
                    MvxTrace.Trace("Warning - activity request code may clash with Mvx code for {0}", (MvxIntentRequestCode)requestCode);
                    break;
                default:
                    // ok...
                    break;
            }
            base.StartActivityForResult(intent, requestCode);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, global::Android.Content.Intent data)
        {
            var handler = MvxIntentResultReceived;
            if (handler != null)
                handler(this, new MvxIntentResultEventArgs(requestCode, resultCode, data));
            base.OnActivityResult(requestCode, resultCode, data);
        }

        #endregion
    }
}