using System;
using Android.Runtime;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Shared.Fragments;
using MvvmCross.Droid.Support.V4;

namespace MvvmCross.Droid.Support.V14.Preference
{
    [Register("mvvmcross.droid.support.v14.preference.MvxPreferenceFragment")]
    public abstract class MvxPreferenceFragment : MvxEventSourcePreferenceFragment, IMvxFragmentView
    {
        private object _dataContext;

        protected MvxPreferenceFragment()
        {
            this.AddEventListeners();
        }

        protected MvxPreferenceFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
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
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        public string UniqueImmutableCacheTag => Tag;

        public virtual void OnViewModelSet()
        {
        }
    }

    public abstract class MvxPreferenceFragment<TViewModel>
        : MvxPreferenceFragment
            , IMvxFragmentView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        protected MvxPreferenceFragment()
        {
        }

        protected MvxPreferenceFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference,
            transfer)
        {
        }


        public new TViewModel ViewModel
        {
            get => (TViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}