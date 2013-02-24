// MvxFragment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Droid.Fragging
{
    public abstract class MvxFragment
        : MvxEventSourceFragment
          , IMvxAndroidFragmentView
    {
        protected MvxFragment()
        {
            this.AddEventListeners();
        }

        public IMvxBaseBindingContext<View> BindingContext { get; set; }

        public object DataContext { get; set; }

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set { DataContext = value; }
        }
    }
}