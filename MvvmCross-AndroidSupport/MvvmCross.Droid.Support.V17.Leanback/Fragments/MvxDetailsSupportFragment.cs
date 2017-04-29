using System;
using Android.OS;
using Android.Runtime;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Shared.Fragments;
using MvvmCross.Droid.Support.V17.Leanback.Fragments.EventSource;
using MvvmCross.Droid.Support.V4;

namespace MvvmCross.Droid.Support.V17.Leanback.Fragments
{
    [Register("mvvmcross.droid.support.v17.leanback.fragments.MvxDetailsSupportFragment")]
    public class MvxDetailsSupportFragment
        : MvxEventSourceDetailsSupportFragment
            , IMvxFragmentView
    {
        private object _dataContext;

        protected MvxDetailsSupportFragment()
        {
            this.AddEventListeners();
        }

        protected MvxDetailsSupportFragment(IntPtr javaReference, JniHandleOwnership transfer)
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

        /// <summary>
        ///     Create new instance of a MvxDetailsSupportFragment
        /// </summary>
        /// <param name="bundle">Usually this would be MvxViewModelRequest serialized</param>
        /// <returns>Returns an instance of a MvxFragment</returns>
        public static MvxDetailsSupportFragment NewInstance(Bundle bundle)
        {
            // Setting Arguments needs to happen before Fragment is attached
            // to Activity. Arguments are persisted when Fragment is recreated!
            var fragment = new MvxDetailsSupportFragment {Arguments = bundle};

            return fragment;
        }

        public virtual void OnViewModelSet()
        {
        }
    }

    public abstract class MvxDetailsSupportFragment<TViewModel>
        : MvxDetailsSupportFragment
            , IMvxFragmentView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        protected MvxDetailsSupportFragment()
        {
        }

        protected MvxDetailsSupportFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference,
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