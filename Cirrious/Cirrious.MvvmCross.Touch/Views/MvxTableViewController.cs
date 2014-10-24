// MvxTableViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxTableViewController
        : MvxEventSourceTableViewController
          , IMvxTouchView
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
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get {
				/*
				Mvx.Trace ("I am in .ViewModel!");
				if (BindingContext == null)
					Mvx.Trace ("BindingContext is null!");
				Mvx.Trace ("I am in .ViewModel 2!");
				if (DataContext == null)
					Mvx.Trace ("DataContext is null!");
				Mvx.Trace ("I am in .ViewModel 3!");

				var c = DataContext;
				Mvx.Trace ("I am in .ViewModel 4!");
				var d = c as IMvxViewModel;
				Mvx.Trace ("I am in .ViewModel 5!");

				var e = (IMvxViewModel)d;
				Mvx.Trace ("I am in .ViewModel 6!");
				if (d == null)
					Mvx.Trace ("d was null!");

				if (e == null)
					Mvx.Trace ("e was null!");
				*/
				return DataContext as IMvxViewModel;
			}
			set { DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }
    }

    public class MvxTableViewController<TViewModel>
        : MvxTableViewController
          , IMvxTouchView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}