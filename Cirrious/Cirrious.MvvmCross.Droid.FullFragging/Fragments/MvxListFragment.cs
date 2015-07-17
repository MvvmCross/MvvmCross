// MvxListFragment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Droid.Fragging.Fragments
{
#warning ListFragment not yet really worked out...
    /*
    public abstract class MvxListFragment
        : MvxEventSourceListFragment
          , IMvxFragmentView
    {
        protected MvxListFragment()
        {
            this.AddEventListeners();
        }

        public IMvxBindingContext<View> BindingContext { get; set; }

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

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set { DataContext = value; }
        }
    }
     */
}