// MvxBaseViewControllerAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Mac.Views
{
    using AppKit;
    using System;

    public class MvxBaseViewControllerAdapter
    {
        private readonly IMvxEventSourceViewController _eventSource;

        protected NSViewController ViewController
        {
            get { return this._eventSource as NSViewController; }
        }

        public MvxBaseViewControllerAdapter(IMvxEventSourceViewController eventSource)
        {
            if (eventSource == null)
                throw new ArgumentException("eventSource - eventSource should not be null");

            if (!(eventSource is NSViewController))
                throw new ArgumentException("eventSource - eventSource should be a NSViewController");

            this._eventSource = eventSource;
            this._eventSource.DisposeCalled += this.HandleDisposeCalled;
            this._eventSource.ViewDidLoadCalled += this.HandleViewDidLoadCalled;
            this._eventSource.ViewDidLayoutCalled += this.HandleViewDidLayoutCalled;
            this._eventSource.ViewWillAppearCalled += this.HandleViewWillAppearCalled;
            this._eventSource.ViewWillAppearCalled += this.HandleViewDidAppearCalled;
            this._eventSource.ViewWillAppearCalled += this.HandleViewWillDisappearCalled;
            this._eventSource.ViewWillAppearCalled += this.HandleViewDidDisappearCalled;
        }

        public virtual void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleViewDidLayoutCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleViewWillAppearCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleViewDidAppearCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleViewWillDisappearCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleViewDidDisappearCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleDisposeCalled(object sender, EventArgs e)
        {
        }
    }
}