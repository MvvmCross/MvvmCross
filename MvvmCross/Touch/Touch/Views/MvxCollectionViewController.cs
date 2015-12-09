// MvxCollectionViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.ViewModels;
using Foundation;
using System;
using UIKit;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxCollectionViewController
        : MvxEventSourceCollectionViewController
          , IMvxTouchView
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
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set { DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }
    }

    public class MvxCollectionViewController<TViewModel>
        : MvxCollectionViewController
          , IMvxTouchView<TViewModel> where TViewModel : class, IMvxViewModel
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
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}