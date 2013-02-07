// MvxTouchTabBarViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Touch.Views
{
	public class EventSourceTabBarController
		: UITabBarController
		, IViewControllerEventSource
	{
		protected EventSourceTabBarController ()
		{			
		}
		
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			ViewWillDisappearCalled.Raise(this, animated);
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			ViewDidDisappearCalled.Raise(this, animated);
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			ViewWillAppearCalled.Raise(this, animated);
		}
		
		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			ViewDidAppearCalled.Raise(this, animated);
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			ViewDidLoadCalled.Raise(this);
		}
		
		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				IsDisposingDisposeCalled.Raise(this);
			}
			base.Dispose (disposing);
		}
		
		public event EventHandler ViewDidLoadCalled;
		public event EventHandler<TypedEventArgs<bool>> ViewWillAppearCalled;
		public event EventHandler<TypedEventArgs<bool>> ViewDidAppearCalled;
		public event EventHandler<TypedEventArgs<bool>> ViewDidDisappearCalled;
		public event EventHandler<TypedEventArgs<bool>> ViewWillDisappearCalled;
		public event EventHandler IsDisposingDisposeCalled;
	}

	public class MvxTabBarViewController
		: EventSourceTabBarController
		, IMvxBindingTouchView
	{
		protected MvxTabBarViewController()
		{
            this.AdaptForBinding();
        }

		public virtual object DataContext { get;set; }
		
		public IMvxViewModel ViewModel
		{
			get { return (IMvxViewModel)DataContext; }
			set { DataContext = value; }
		}
		
		public bool IsVisible
		{
			get { return this.IsVisible(); }
		}
		
		public MvxShowViewModelRequest ShowRequest { get; set; }

        private readonly List<IMvxUpdateableBinding> _bindings = new List<IMvxUpdateableBinding>();

        public List<IMvxUpdateableBinding> Bindings
        {
            get { return _bindings; }
        }
    }
}