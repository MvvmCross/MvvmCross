// MvxDialogFragment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat.EventSource;
using MvvmCross.Droid.Views;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    [Register("mvvmcross.droid.support.v7.appcompat.MvxAppCompatDialogFragment")]
    public abstract class MvxAppCompatDialogFragment
        : MvxEventSourceAppCompatDialogFragment, IMvxFragmentView
    {
        protected MvxAppCompatDialogFragment()
        {
            this.AddEventListeners();
        }

        protected MvxAppCompatDialogFragment(IntPtr javaReference, JniHandleOwnership transfer)
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
            ViewModel?.ViewDestroy();
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

    public abstract class MvxAppCompatDialogFragment<TViewModel>
        : MvxAppCompatDialogFragment, IMvxFragmentView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected MvxAppCompatDialogFragment()
        {
        }

        protected MvxAppCompatDialogFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}