using Android.Runtime;
using MvvmCross.Binding.BindingContext;

using MvvmCross.Core.ViewModels;
using System;
using MvvmCross.Droid.Shared.Fragments;
using MvvmCross.Droid.Support.V4;

namespace MvvmCross.Droid.Support.V7.Preference
{
    public abstract class MvxPreferenceFragmentCompat : MvxEventSourcePreferenceFragmentCompat, IMvxFragmentView
    {
        protected MvxPreferenceFragmentCompat()
        {
            this.AddEventListeners();
        }

        protected MvxPreferenceFragmentCompat(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{}

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
        : MvxPreferenceFragmentCompat
    , IMvxFragmentView<TViewModel> where TViewModel : class, IMvxViewModel
    {

        protected MvxPreferenceFragmentCompat()
        {

        }

        protected MvxPreferenceFragmentCompat(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }


        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}