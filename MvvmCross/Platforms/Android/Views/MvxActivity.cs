// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using Android.Content;
using Android.Runtime;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Views.Base;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Android.Views
{
    [Register("mvvmcross.platforms.android.views.MvxActivity")]
    public abstract class MvxActivity
        : MvxEventSourceActivity
        , IMvxAndroidView
    {
        protected MvxActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected MvxActivity()
        {
            BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
        }

        public object? DataContext
        {
            get => BindingContext?.DataContext;
            set
            {
                if (BindingContext != null)
                    BindingContext.DataContext = value;
            }
        }

        public IMvxViewModel? ViewModel
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

        public IMvxBindingContext? BindingContext { get; set; }

        // ReSharper disable once InconsistentNaming
        public override void SetContentView(int layoutResID)
        {
            if (BaseContextToAttach(this) is MvxContextWrapper)
            {
                var view = this.BindingInflate(layoutResID, null);
                SetContentView(view);
                return;
            }

            base.SetContentView(layoutResID);
        }

        protected virtual void OnViewModelSet()
        {
        }

        protected virtual Context BaseContextToAttach(Context? @base)
            => MvxContextWrapper.Wrap(@base, this);

        protected override void AttachBaseContext(Context? @base)
        {
            if (this is IMvxSetupMonitor)
            {
                // Do not attach our inflater to splash screens.
                base.AttachBaseContext(@base);
                return;
            }
            base.AttachBaseContext(BaseContextToAttach(@base));
        }

        protected override void OnCreate(Bundle? savedInstanceState)
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
        public new TViewModel? ViewModel
        {
            get => (TViewModel?)base.ViewModel;
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
