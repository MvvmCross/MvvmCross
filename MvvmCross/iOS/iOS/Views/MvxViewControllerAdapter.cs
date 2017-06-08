// MvxViewControllerAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Views
{
    using System;

    using Core.Views;
    using MvvmCross.Platform.iOS.Views;

    public class MvxViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        protected IMvxIosView IosView => ViewController as IMvxIosView;

        public MvxViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxIosView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxIosView");
        }

        public override void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
            IosView.OnViewCreate();
            base.HandleViewDidLoadCalled(sender, e);
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            IosView.OnViewDestroy();
            base.HandleDisposeCalled(sender, e);
        }
    }
}