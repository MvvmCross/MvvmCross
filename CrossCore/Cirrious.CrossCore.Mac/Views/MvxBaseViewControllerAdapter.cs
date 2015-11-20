// MvxBaseViewControllerAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

#if __UNIFIED__
using AppKit;
#else
#endif

namespace Cirrious.CrossCore.Mac.Views
{
    public class MvxBaseViewControllerAdapter
    {
        private readonly IMvxEventSourceViewController _eventSource;

        protected NSViewController ViewController
        {
            get { return _eventSource as NSViewController; }
        }

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

        public virtual void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleDisposeCalled(object sender, EventArgs e)
        {
        }
    }
}