// MvxTabBarViewController.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Touch.Views
{
    using System;

    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform.Touch.Views;

    public class MvxTabBarViewController
        : MvxEventSourceTabBarController
          , IMvxTouchView
    {
        protected MvxTabBarViewController()
        {
            this.AdaptForBinding();
        }

        protected MvxTabBarViewController(IntPtr handle)
            : base(handle)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get
            {
                // special code needed in TabBar because View is initialized during construction
                return this.BindingContext?.DataContext;
            }
            set { this.BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return this.DataContext as IMvxViewModel; }
            set { this.DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }
    }

    public class MvxTabBarViewController<TViewModel>
        : MvxTabBarViewController
          , IMvxTouchView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        protected MvxTabBarViewController()
        {
        }

        protected MvxTabBarViewController(IntPtr handle)
            : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}