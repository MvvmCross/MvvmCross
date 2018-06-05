﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Runtime;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;

namespace MvvmCross.Droid.Support.V7.Preference
{
    [Register("mvvmcross.droid.support.v7.preference.MvxPreferenceFragmentCompat")]
    public abstract class MvxPreferenceFragmentCompat 
        : MvxEventSourcePreferenceFragmentCompat, IMvxFragmentView
    {
        protected MvxPreferenceFragmentCompat()
        {
            V4.MvxFragmentExtensions.AddEventListeners(this);
        }

        protected MvxPreferenceFragmentCompat(IntPtr javaReference, JniHandleOwnership transfer)
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

    public abstract class MvxPreferenceFragmentCompat<TViewModel>
        : MvxPreferenceFragmentCompat, IMvxFragmentView<TViewModel> 
        where TViewModel : class, IMvxViewModel
    {
        protected MvxPreferenceFragmentCompat()
        {
        }

        protected MvxPreferenceFragmentCompat(IntPtr javaReference, JniHandleOwnership transfer)
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
