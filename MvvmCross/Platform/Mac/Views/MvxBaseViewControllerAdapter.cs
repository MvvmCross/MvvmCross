// MvxBaseViewControllerAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#if __UNIFIED__
using AppKit;
#else
#endif

namespace MvvmCross.Platform.Mac.Views
{
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
        }

        public virtual void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleDisposeCalled(object sender, EventArgs e)
        {
        }
    }
}