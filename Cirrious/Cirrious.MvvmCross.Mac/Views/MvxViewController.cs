// <copyright file="MvxTouchViewController.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

using Cirrious.CrossCore.Mac.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.ViewModels;
using System;

#if __UNIFIED__
using AppKit;
using Foundation;
#else
#endif

namespace Cirrious.MvvmCross.Mac.Views
{
    public class MvxViewController
        : MvxEventSourceViewController
            , IMvxMacView
    {
        // Called when created from unmanaged code
        public MvxViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MvxViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public MvxViewController(string viewName, NSBundle bundle) : base(viewName, bundle)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public MvxViewController(string viewName) : base(viewName, NSBundle.MainBundle)
        {
            Initialize();
        }

        public MvxViewController() : base()
        {
            Initialize();
        }

        // Shared initialization code
        private void Initialize()
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
            get { return (IMvxViewModel)DataContext; }
            set { DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }
    }
}