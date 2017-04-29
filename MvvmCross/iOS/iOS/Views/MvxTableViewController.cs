// MvxTableViewController.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.iOS.Views;
using UIKit;

namespace MvvmCross.iOS.Views
{
    public class MvxTableViewController
        : MvxEventSourceTableViewController
            , IMvxIosView
    {
        protected MvxTableViewController(UITableViewStyle style = UITableViewStyle.Plain)
            : base(style)
        {
            this.AdaptForBinding();
        }

        protected MvxTableViewController(IntPtr handle)
            : base(handle)
        {
            this.AdaptForBinding();
        }

        protected MvxTableViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get => BindingContext.DataContext;
            set => BindingContext.DataContext = value;
        }

        public IMvxViewModel ViewModel
        {
            get => DataContext as IMvxViewModel;
            set => DataContext = value;
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }
    }

    public class MvxTableViewController<TViewModel>
        : MvxTableViewController
            , IMvxIosView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        protected MvxTableViewController(UITableViewStyle style = UITableViewStyle.Plain)
            : base(style)
        {
        }

        protected MvxTableViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected MvxTableViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
        }

        public new TViewModel ViewModel
        {
            get => (TViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}