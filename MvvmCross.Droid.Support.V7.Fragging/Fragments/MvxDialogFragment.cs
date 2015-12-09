// MvxDialogFragment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using Android.Runtime;
using Cirrious.MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.Fragging.Fragments.EventSource;
using Cirrious.MvvmCross.ViewModels;
using System;

namespace MvvmCross.Droid.Support.V7.Fragging.Fragments
{
    public abstract class MvxDialogFragment
        : MvxEventSourceDialogFragment
        , IMvxFragmentView
    {
        protected MvxDialogFragment()
        {
            this.AddEventListeners();
        }

        protected MvxDialogFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            this.AddEventListeners();
        }

        public IMvxBindingContext BindingContext { get; set; }

        private object _dataContext;

        public object DataContext
        {
            get { return _dataContext; }
            set
            {
                _dataContext = value;
                if (BindingContext != null)
                    BindingContext.DataContext = value;
            }
        }

        public virtual IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set { DataContext = value; }
        }

        protected void EnsureBindingContextSet(Bundle b0)
        {
            this.EnsureBindingContextIsSet(b0);
        }

        public virtual string UniqueImmutableCacheTag => Tag;
    }

    public abstract class MvxDialogFragment<TViewModel>
        : MvxDialogFragment
        , IMvxFragmentView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}