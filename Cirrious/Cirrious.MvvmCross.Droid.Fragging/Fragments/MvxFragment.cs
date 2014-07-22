// MvxFragment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments.EventSource;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Fragging.Fragments
{
    public class MvxFragment
        : MvxEventSourceFragment
        , IMvxFragmentView
    {
        public static MvxFragment NewInstance(Bundle bundle)
        {
            var fragment = new MvxFragment { Arguments = bundle };

            return fragment;
        }

        protected MvxFragment()
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
    }
}