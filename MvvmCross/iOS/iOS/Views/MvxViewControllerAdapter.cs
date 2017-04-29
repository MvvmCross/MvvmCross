// MvxViewControllerAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Core.Views;
using MvvmCross.Platform.iOS.Views;

namespace MvvmCross.iOS.Views
{
    public class MvxViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        public MvxViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxIosView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxIosView");
        }

        protected IMvxIosView IosView => ViewController as IMvxIosView;

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