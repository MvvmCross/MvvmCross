using Android.OS;
using Android.Runtime;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V17.Leanback.Fragments.EventSource;
using System;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;
using MvvmCross.Droid.Shared.Fragments;

namespace MvvmCross.Droid.Support.V17.Leanback.Fragments
{
    public class MvxPlaybackOverlaySupportFragment
        : MvxEventSourcePlaybackOverlaySupportFragment
            , IMvxFragmentView
    {
        /// <summary>
        /// Create new instance of a MvxSearchSupportFragment
        /// </summary>
        /// <param name="bundle">Usually this would be MvxViewModelRequest serialized</param>
        /// <returns>Returns an instance of a MvxFragment</returns>
        public static MvxPlaybackOverlaySupportFragment NewInstance(Bundle bundle)
        {
            // Setting Arguments needs to happen before Fragment is attached
            // to Activity. Arguments are persisted when Fragment is recreated!
            var fragment = new MvxPlaybackOverlaySupportFragment { Arguments = bundle };

            return fragment;
        }

        protected MvxPlaybackOverlaySupportFragment()
        {
            this.AddEventListeners();
        }

        protected MvxPlaybackOverlaySupportFragment(IntPtr javaReference, JniHandleOwnership transfer)
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

    public abstract class MvxPlaybackOverlaySupportFragment<TViewModel>
: MvxPlaybackOverlaySupportFragment
    , IMvxFragmentView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        protected MvxPlaybackOverlaySupportFragment()
        {
        }

        protected MvxPlaybackOverlaySupportFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}