// MvxTabBarViewController.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Views
{
    using System;
    using Foundation;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform.iOS.Views;
    using UIKit;

    public class MvxTabBarViewController
        : MvxEventSourceTabBarController
          , IMvxIosView
          , IMvxIosViewSegue
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

        // CHECKME: won't this interfer with the hack for TabBar in LoadViewModel()
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }

        public virtual object PrepareViewModelParametersForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            return null;
        }
    }

    public class MvxTabBarViewController<TViewModel>
        : MvxTabBarViewController
          , IMvxIosView<TViewModel> where TViewModel : class, IMvxViewModel
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