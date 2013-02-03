// MvxTouchViewController.cs
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
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform;

namespace Cirrious.MvvmCross.Touch.Views
{
	public class TypedEventArgs<T> : EventArgs
	{
		public TypedEventArgs(T value)
		{
			Value = value;
		}
		
		public T Value { get; private set; }
	}
	
	public interface IDisposeSource
	{
		event EventHandler IsDisposingDisposeCalled;
	}

	public interface IViewControllerEventSource : IDisposeSource
	{
		event EventHandler ViewDidLoadCalled;
		event EventHandler<TypedEventArgs<bool>> ViewWillAppearCalled;
		event EventHandler<TypedEventArgs<bool>> ViewDidAppearCalled;
		event EventHandler<TypedEventArgs<bool>> ViewDidDisappearCalled;
		event EventHandler<TypedEventArgs<bool>> ViewWillDisappearCalled;
	}

	public static class MvxDelegateExtensionMethods
	{
		public static void Raise (this EventHandler eventHandler, object sender)
		{
			if (eventHandler == null)
				return;

			eventHandler(sender, EventArgs.Empty);
		}

		public static void Raise<T> (this EventHandler<TypedEventArgs<T>> eventHandler, object sender, T value)
		{
			if (eventHandler == null)
				return;
			
			eventHandler(sender, new TypedEventArgs<T>(value));
		}
	}	

	public class EventSourceCollectionViewController
		: UICollectionViewController
		, IViewControllerEventSource
	{
		protected EventSourceCollectionViewController (UICollectionViewLayout layout)
			: base(layout)
		{			
			var adapter = new MvxViewControllerAdapter(this);	
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

	public class EventSourceTableViewController
		: UITableViewController
			, IViewControllerEventSource
	{
		protected EventSourceTableViewController (UITableViewStyle style = UITableViewStyle.Plain)
			: base(style)
		{			
			var adapter = new MvxViewControllerAdapter(this);	
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

	public class EventSourceViewController
		: UIViewController
		, IViewControllerEventSource
	{
		protected EventSourceViewController ()
		{			
			var adapter = new MvxViewControllerAdapter(this);	
		}

		protected EventSourceViewController(string nibName, NSBundle bundle)
			: base(nibName, bundle)
		{
			var adapter = new MvxViewControllerAdapter(this);	
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

	public class BaseViewControllerAdapter
	{
		private IViewControllerEventSource _eventSource;

		protected UIViewController ViewController {
			get {
				return _eventSource as UIViewController;
			}
		}

		protected IMvxTouchView TouchView {
			get {
				return _eventSource as IMvxTouchView;
			}
		}

		public BaseViewControllerAdapter (IViewControllerEventSource eventSource)
		{
			if (eventSource == null)
				throw new ArgumentException("eventSource", "eventSource should not be null");

			if (!(eventSource is UIViewController))
				throw new ArgumentException("eventSource", "eventSource should be a UIViewController");

			if (!(eventSource is IMvxTouchView))
				throw new ArgumentException("eventSource", "eventSource should be a IMvxTouchView");

			_eventSource = eventSource;
			_eventSource.ViewDidAppearCalled += HandleViewDidAppearCalled;
			_eventSource.ViewDidDisappearCalled += HandleViewDidDisappearCalled;
			_eventSource.ViewWillAppearCalled += HandleViewWillAppearCalled;
			_eventSource.ViewWillDisappearCalled += HandleViewWillDisappearCalled;
			_eventSource.IsDisposingDisposeCalled += HandleIsDisposingDisposeCalled;
			_eventSource.ViewDidLoadCalled += HandleViewDidLoadCalled;
		}

		public virtual void HandleViewDidLoadCalled (object sender, EventArgs e)
		{			
		}

		public virtual void HandleIsDisposingDisposeCalled (object sender, EventArgs e)
		{
		}

		public virtual void HandleViewWillDisappearCalled (object sender, TypedEventArgs<bool> e)
		{
		}

		public virtual void HandleViewWillAppearCalled (object sender, TypedEventArgs<bool> e)
		{
		}

		public virtual void HandleViewDidDisappearCalled (object sender, TypedEventArgs<bool> e)
		{		
		}

		public virtual void HandleViewDidAppearCalled (object sender, TypedEventArgs<bool> e)
		{		
		}
	}

	public class MvxViewControllerAdapter : BaseViewControllerAdapter
	{
		public MvxViewControllerAdapter (IViewControllerEventSource eventSource)
			: base(eventSource)
		{
		}

		public override void HandleViewDidLoadCalled (object sender, EventArgs e)
		{
			TouchView.OnViewCreate();
			base.HandleViewDidLoadCalled (sender, e);
		}

		public override void HandleIsDisposingDisposeCalled (object sender, EventArgs e)
		{
			TouchView.OnViewDestroy();
			base.HandleIsDisposingDisposeCalled (sender, e);
		}
	}

	public class MvxViewController : EventSourceViewController, IMvxTouchView
	{
		public MvxViewController ()
		{
			var adapter = new MvxViewControllerAdapter(this);	
		}

		protected MvxViewController(string nibName, NSBundle bundle)
			: base(nibName, bundle)
		{
			var adapter = new MvxViewControllerAdapter(this);	
		}

		public IMvxViewModel ViewModel { get;set; }
		
		public bool IsVisible
		{
			get { return this.IsVisible(); }
		}
		
		public MvxShowViewModelRequest ShowRequest { get; set; }
	}

    public class MvxTouchViewController<TViewModel>
        : UIViewController
          , IMvxTouchView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
		protected MvxTouchViewController(MvxShowViewModelRequest request)
        {
            ShowRequest = request;
        }

        protected MvxTouchViewController(MvxShowViewModelRequest request, string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            ShowRequest = request;
        }

        #region Shared code across all Touch ViewControllers

        private IMvxViewModel _viewModel;

        public Type ViewModelType
        {
            get { return typeof (TViewModel); }
        }

        public TViewModel ViewModel
        {
            get { return (TViewModel)((IMvxView)this).ViewModel; }
            set
            {
				((IMvxView)this).ViewModel = value;
            }
        }

		IMvxViewModel IMvxView.ViewModel
		{
			get { return _viewModel; }
			set
			{
				_viewModel = (TViewModel)value;
				OnViewModelChanged();
			}
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
            this.OnViewCreate();
            base.ViewDidLoad();
        }

        #endregion
    }
}