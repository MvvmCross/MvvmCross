using System;
using Cirrious.MvvmCross.Droid.Views;
using MonoTouch.UIKit;

namespace Cirrious.CrossCore.Touch.Views
{
    public class BaseViewControllerAdapter
    {
        private IViewControllerEventSource _eventSource;

        protected UIViewController ViewController
        {
            get
            {
                return _eventSource as UIViewController;
            }
        }

        public BaseViewControllerAdapter(IViewControllerEventSource eventSource)
        {
            if (eventSource == null)
                throw new ArgumentException("eventSource", "eventSource should not be null");

            if (!(eventSource is UIViewController))
                throw new ArgumentException("eventSource", "eventSource should be a UIViewController");

            _eventSource = eventSource;
            _eventSource.ViewDidAppearCalled += HandleViewDidAppearCalled;
            _eventSource.ViewDidDisappearCalled += HandleViewDidDisappearCalled;
            _eventSource.ViewWillAppearCalled += HandleViewWillAppearCalled;
            _eventSource.ViewWillDisappearCalled += HandleViewWillDisappearCalled;
            _eventSource.DisposeCalled += HandleDisposeCalled;
            _eventSource.ViewDidLoadCalled += HandleViewDidLoadCalled;
        }

        public virtual void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleDisposeCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleViewWillDisappearCalled(object sender, TypedEventArgs<bool> e)
        {
        }

        public virtual void HandleViewWillAppearCalled(object sender, TypedEventArgs<bool> e)
        {
        }

        public virtual void HandleViewDidDisappearCalled(object sender, TypedEventArgs<bool> e)
        {
        }

        public virtual void HandleViewDidAppearCalled(object sender, TypedEventArgs<bool> e)
        {
        }
    }
}