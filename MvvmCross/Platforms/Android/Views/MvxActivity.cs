// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Views.Base;
using MvvmCross.ViewModels;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace MvvmCross.Platforms.Android.Views
{
    [Register("mvvmcross.platforms.android.views.MvxActivity")]
    public abstract class MvxActivity
        : MvxEventSourceActivity
        , IMvxAndroidView
    {
        protected View _view;

        protected MvxActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected MvxActivity()
        {
            BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
        }

        public object DataContext
        {
            get => BindingContext.DataContext;
            set => BindingContext.DataContext = value;
        }

        public IMvxViewModel ViewModel
        {
            get => DataContext as IMvxViewModel;
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResult(intent, requestCode);
        }

        public IMvxBindingContext BindingContext { get; set; }

        public override void SetContentView(int layoutResID)
        {
            if (BaseContextToAttach(this) is MvxContextWrapper)
            {
                _view = this.BindingInflate(layoutResID, null);
                SetContentView(_view);
                return;
            }

            base.SetContentView(layoutResID);
        }

        protected virtual void OnViewModelSet()
        {
        }

        protected virtual Context BaseContextToAttach(Context @base)
            => MvxContextWrapper.Wrap(@base, this);

        protected override void AttachBaseContext(Context @base)
        {
            if (this is IMvxSetupMonitor)
            {
                // Do not attach our inflater to splash screens.
                base.AttachBaseContext(@base);
                return;
            }
            base.AttachBaseContext(BaseContextToAttach(@base));
        }

        private readonly List<WeakReference<Fragment>> _fragList = new List<WeakReference<Fragment>>();

        public override void OnAttachFragment(Fragment fragment)
        {
            base.OnAttachFragment(fragment);
            _fragList.Add(new WeakReference<Fragment>(fragment));
        }

        public List<Fragment> Fragments
        {
            get
            {
                var fragments = new List<Fragment>();
                foreach (var weakReference in _fragList)
                {
                    if (weakReference.TryGetTarget(out Fragment f))
                    {
                        if (f.IsVisible)
                            fragments.Add(f);
                    }
                }

                return fragments;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ViewModel?.ViewCreated();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ViewModel?.ViewDestroy(IsFinishing);
        }

        protected override void OnStart()
        {
            base.OnStart();
            ViewModel?.ViewAppearing();
        }

        protected override void OnResume()
        {
            base.OnResume();
            ViewModel?.ViewAppeared();
        }

        protected override void OnPause()
        {
            base.OnPause();
            ViewModel?.ViewDisappearing();
        }

        protected override void OnStop()
        {
            base.OnStop();
            ViewModel?.ViewDisappeared();
        }
    }

    public abstract class MvxActivity<TViewModel> : MvxActivity, IMvxAndroidView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get => (TViewModel)base.ViewModel;
            set => base.ViewModel = value;
        }

        protected MvxActivity()
        {
        }

        protected MvxActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public MvxFluentBindingDescriptionSet<IMvxAndroidView<TViewModel>, TViewModel> CreateBindingSet()
            => this.CreateBindingSet<IMvxAndroidView<TViewModel>, TViewModel>();
    }
}
