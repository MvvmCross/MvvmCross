// MvxTouchTabBarViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
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
		, IMvxTouchView
	{
		protected MvxTabBarViewController()
		{
			var adapter = new MvxViewControllerAdapter(this);
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
	}


    public class MvxTouchTabBarViewController<TViewModel>
        : UITabBarController
          , IMvxTouchView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected MvxTouchTabBarViewController(MvxShowViewModelRequest request)
        {
            ShowRequest = request;
        }

        #region Shared code across all Touch ViewControllers

		public Type ViewModelType
		{
			get { return typeof (TViewModel); }
		}

		public virtual object DataContext { get; set; }

		public TViewModel ViewModel
		{
			get { return (TViewModel)DataContext; }
			set { DataContext = value; }
		}
		
		IMvxViewModel IMvxView.ViewModel
		{
			get { return (IMvxViewModel)DataContext; }
			set { DataContext = value; }
		}

		public bool IsVisible
		{
			get { return this.IsVisible(); }
		}		

        public MvxShowViewModelRequest ShowRequest { get; set; }

        protected virtual void OnViewModelChanged()
        {
        }

#warning really need to think about how to handle ios6 once ViewDidUnload has been removed
        [Obsolete]
        public override void ViewDidUnload()
        {
            this.OnViewDestroy();
            base.ViewDidUnload();
        }

        public override void ViewDidLoad()
        {
            if (ShowRequest == null)
                return;
            this.OnViewCreate();
            base.ViewDidLoad();
        }

        #endregion
    }
}