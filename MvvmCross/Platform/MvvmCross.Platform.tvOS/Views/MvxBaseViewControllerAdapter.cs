// MvxBaseViewControllerAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.iOS.Views
{
    using System;

    using MvvmCross.Platform.Core;

    using UIKit;

    public class MvxBaseViewControllerAdapter
    {
        private readonly IMvxEventSourceViewController _eventSource;

        protected UIViewController ViewController => this._eventSource as UIViewController;

        public MvxBaseViewControllerAdapter(IMvxEventSourceViewController eventSource)
        {
            if (eventSource == null)
                throw new ArgumentException("eventSource - eventSource should not be null");

            if (!(eventSource is UIViewController))
                throw new ArgumentException("eventSource - eventSource should be a UIViewController");

            this._eventSource = eventSource;
            this._eventSource.ViewDidAppearCalled += this.HandleViewDidAppearCalled;
            this._eventSource.ViewDidDisappearCalled += this.HandleViewDidDisappearCalled;
            this._eventSource.ViewWillAppearCalled += this.HandleViewWillAppearCalled;
            this._eventSource.ViewWillDisappearCalled += this.HandleViewWillDisappearCalled;
            this._eventSource.DisposeCalled += this.HandleDisposeCalled;
            this._eventSource.ViewDidLoadCalled += this.HandleViewDidLoadCalled;
            this._eventSource.ViewDidLayoutSubviewsCalled += this.HandleViewDidLayoutSubviewsCalled;
        }

        public virtual void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleViewDidLayoutSubviewsCalled(object sender, EventArgs e)
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