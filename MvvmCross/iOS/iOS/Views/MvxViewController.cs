// MvxViewController.cs

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

    public class MvxViewController
        : MvxEventSourceViewController
          , IMvxIosView
    {
        public MvxViewController()
        {
            this.AdaptForBinding();
        }

        public MvxViewController(IntPtr handle)
            : base(handle)
        {
            this.AdaptForBinding();
        }

        protected MvxViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get { return this.BindingContext.DataContext; }
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

    public class MvxViewController<TViewModel>
        : MvxViewController
          , IMvxIosView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public MvxViewController()
        {
        }

        public MvxViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected MvxViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender) {
            base.PrepareForSegue(segue, sender);
            this.InstantiateViewModelForSegue(segue);
        }
    }
}