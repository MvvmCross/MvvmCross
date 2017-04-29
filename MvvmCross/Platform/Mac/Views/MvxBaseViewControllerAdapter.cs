// MvxBaseViewControllerAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


using System;
using AppKit;

namespace MvvmCross.Platform.Mac.Views
{
    public class MvxBaseViewControllerAdapter
    {
        private readonly IMvxEventSourceViewController _eventSource;

        public MvxBaseViewControllerAdapter(IMvxEventSourceViewController eventSource)
        {
            if (eventSource == null)
                throw new ArgumentException("eventSource - eventSource should not be null");

            if (!(eventSource is NSViewController))
                throw new ArgumentException("eventSource - eventSource should be a NSViewController");

            _eventSource = eventSource;
            _eventSource.DisposeCalled += HandleDisposeCalled;
            _eventSource.ViewDidLoadCalled += HandleViewDidLoadCalled;
        }

        protected NSViewController ViewController => _eventSource as NSViewController;

        public virtual void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleDisposeCalled(object sender, EventArgs e)
        {
        }
    }
}