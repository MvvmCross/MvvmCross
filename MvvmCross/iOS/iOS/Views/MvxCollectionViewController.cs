// MvxCollectionViewController.cs

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
    public class MvxCollectionViewController
        : MvxEventSourceCollectionViewController
            , IMvxIosView
    {
        protected MvxCollectionViewController(UICollectionViewLayout layout)
            : base(layout)
        {
            this.AdaptForBinding();
        }

        protected MvxCollectionViewController(IntPtr handle)
            : base(handle)
        {
            this.AdaptForBinding();
        }

        protected MvxCollectionViewController(string nibName, NSBundle bundle)
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

    public class MvxCollectionViewController<TViewModel>
        : MvxCollectionViewController
            , IMvxIosView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        protected MvxCollectionViewController(UICollectionViewLayout layout) : base(layout)
        {
        }

        protected MvxCollectionViewController(IntPtr handle) : base(handle)
        {
        }

        protected MvxCollectionViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public new TViewModel ViewModel
        {
            get => (TViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}