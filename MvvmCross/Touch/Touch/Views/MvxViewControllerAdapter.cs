// MvxViewControllerAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Touch.Views
{
    using System;

    using MvvmCross.Core.Views;
    using MvvmCross.Platform.Touch.Views;

    public class MvxViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        protected IMvxTouchView TouchView => base.ViewController as IMvxTouchView;

        public MvxViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxTouchView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxTouchView");
        }

        public override void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
            this.TouchView.OnViewCreate();
            base.HandleViewDidLoadCalled(sender, e);
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            this.TouchView.OnViewDestroy();
            base.HandleDisposeCalled(sender, e);
        }
    }
}