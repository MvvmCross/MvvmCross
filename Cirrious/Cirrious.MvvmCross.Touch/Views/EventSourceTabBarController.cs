// MvxTouchTabBarViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Touch.Interfaces;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Touch.Views
{
	public class EventSourceTabBarController
		: UITabBarController
		, IMvxEventSourceViewController
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
				DisposeCalled.Raise(this);
			}
			base.Dispose (disposing);
		}
		
		public event EventHandler ViewDidLoadCalled;
		public event EventHandler<MvxTypedEventArgs<bool>> ViewWillAppearCalled;
		public event EventHandler<MvxTypedEventArgs<bool>> ViewDidAppearCalled;
		public event EventHandler<MvxTypedEventArgs<bool>> ViewDidDisappearCalled;
		public event EventHandler<MvxTypedEventArgs<bool>> ViewWillDisappearCalled;
		public event EventHandler DisposeCalled;
	}
}