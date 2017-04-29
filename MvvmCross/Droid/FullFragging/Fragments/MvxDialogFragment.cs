// MvxDialogFragment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.OS;
using Android.Runtime;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.FullFragging.Fragments.EventSource;
using MvvmCross.Droid.Shared.Fragments;

namespace MvvmCross.Droid.FullFragging.Fragments
{
    [Register("mvvmcross.droid.fullfragging.fragments.MvxDialogFragment")]
    public abstract class MvxDialogFragment
        : MvxEventSourceDialogFragment
            , IMvxFragmentView
    {
        private object _dataContext;

        protected MvxDialogFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected MvxDialogFragment()
        {
            this.AddEventListeners();
        }

        public IMvxBindingContext BindingContext { get; set; }

        public object DataContext
        {
            get => _dataContext;
            set
            {
                _dataContext = value;
                if (BindingContext != null)
                    BindingContext.DataContext = value;
            }
        }

        public virtual IMvxViewModel ViewModel
        {
            get => DataContext as IMvxViewModel;
            set => DataContext = value;
        }

        public string UniqueImmutableCacheTag => Tag;

        protected void EnsureBindingContextSet(Bundle b0)
        {
            this.EnsureBindingContextIsSet(b0);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            ViewModel?.Destroy();
        }

        public override void OnStart()
        {
            base.OnStart();
            ViewModel.Appearing();
        }

        public override void OnResume()
        {
            base.OnResume();
            ViewModel.Appeared();
        }

        public override void OnPause()
        {
            base.OnPause();
            ViewModel.Disappearing();
        }

        public override void OnStop()
        {
            base.OnStop();
            ViewModel.Disappeared();
        }
    }

    public abstract class MvxDialogFragment<TViewModel>
        : MvxDialogFragment
            , IMvxFragmentView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get => (TViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}