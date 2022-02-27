// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.Material.EventSource;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;

namespace MvvmCross.DroidX.Material
{
    [Register("mvvmcross.droidx.material.MvxBottomSheetDialogFragment")]
    public abstract class MvxBottomSheetDialogFragment
        : MvxEventSourceBottomSheetDialogFragment, IMvxFragmentView
    {
        protected MvxBottomSheetDialogFragment()
        {
            var _ = new MvxBindingFragmentAdapter(this);
        }

        protected MvxBottomSheetDialogFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public IMvxBindingContext BindingContext { get; set; }

        private object _dataContext;
        public object DataContext
        {
            get
            {
                return _dataContext;
            }
            set
            {
                _dataContext = value;
                if (BindingContext != null)
                    BindingContext.DataContext = value;
            }
        }

        public virtual IMvxViewModel ViewModel
        {
            get
            {
                return DataContext as IMvxViewModel;
            }
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        public virtual void OnViewModelSet()
        {
        }

        public virtual string UniqueImmutableCacheTag => Tag;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ViewModel?.ViewCreated();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            ViewModel?.ViewDestroy(viewFinishing: IsRemoving || Activity == null || Activity.IsFinishing);
        }

        public override void OnStart()
        {
            base.OnStart();
            ViewModel?.ViewAppearing();
        }

        public override void OnResume()
        {
            base.OnResume();
            ViewModel?.ViewAppeared();
        }

        public override void OnPause()
        {
            base.OnPause();
            ViewModel?.ViewDisappearing();
        }

        public override void OnStop()
        {
            base.OnStop();
            ViewModel?.ViewDisappeared();
        }

        public override void OnCancel(IDialogInterface dialog)
        {
            base.OnCancel(dialog);
            ViewModel?.ViewDestroy();
        }

        public override void DismissAllowingStateLoss()
        {
            base.DismissAllowingStateLoss();
            ViewModel?.ViewDestroy();
        }

        public override void Dismiss()
        {
            base.Dismiss();
            ViewModel?.ViewDestroy();
        }
    }

    public abstract class MvxBottomSheetDialogFragment<TViewModel> : MvxBottomSheetDialogFragment, IMvxFragmentView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        public MvxBottomSheetDialogFragment()
        {
        }

        protected MvxBottomSheetDialogFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxFluentBindingDescriptionSet<IMvxFragmentView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxFragmentView<TViewModel>, TViewModel>();
        }
    }
}
