﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.OS;
using Android.Runtime;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.Leanback.Fragments.EventSource;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;

namespace MvvmCross.DroidX.Leanback.Fragments
{
    [Register("mvvmcross.droidx.leanback.fragments.MvxGuidedStepSupportFragment")]
    public class MvxGuidedStepSupportFragment
        : MvxEventSourceGuidedStepSupportFragment, IMvxFragmentView
    {
        /// <summary>
        /// Create new instance of a MvxGuidedStepSupportFragment
        /// </summary>
        /// <param name="bundle">Usually this would be MvxViewModelRequest serialized</param>
        /// <returns>Returns an instance of a MvxFragment</returns>
        public static MvxGuidedStepSupportFragment NewInstance(Bundle bundle)
        {
            // Setting Arguments needs to happen before Fragment is attached
            // to Activity. Arguments are persisted when Fragment is recreated!
            var fragment = new MvxGuidedStepSupportFragment { Arguments = bundle };

            return fragment;
        }

        protected MvxGuidedStepSupportFragment()
        {
            var _ = new MvxBindingFragmentAdapter(this);
        }

        protected MvxGuidedStepSupportFragment(IntPtr javaReference, JniHandleOwnership transfer)
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

        public string UniqueImmutableCacheTag => Tag;
    }

    public abstract class MvxGuidedStepSupportFragment<TViewModel> : MvxGuidedStepSupportFragment, IMvxFragmentView<TViewModel> 
        where TViewModel : class, IMvxViewModel
    {
        protected MvxGuidedStepSupportFragment()
        {
        }

        protected MvxGuidedStepSupportFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
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
