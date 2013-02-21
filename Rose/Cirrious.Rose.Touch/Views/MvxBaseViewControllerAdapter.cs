using System;
using Cirrious.CrossCore.Interfaces.Core;
using MonoTouch.UIKit;

namespace Cirrious.CrossCore.Touch.Views
{
    public class MvxBaseViewControllerAdapter
    {
        private IMvxEventSourceViewController _eventSource;

        protected UIViewController ViewController
        {
            get
            {
                return _eventSource as UIViewController;
            }
        }

        public MvxBaseViewControllerAdapter(IMvxEventSourceViewController eventSource)
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

        public virtual void HandleViewWillDisappearCalled(object sender, MvxValueEventArgs<bool> e)
        {
        }

        public virtual void HandleViewWillAppearCalled(object sender, MvxValueEventArgs<bool> e)
        {
        }

        public virtual void HandleViewDidDisappearCalled(object sender, MvxValueEventArgs<bool> e)
        {
        }

        public virtual void HandleViewDidAppearCalled(object sender, MvxValueEventArgs<bool> e)
        {
        }
    }
}