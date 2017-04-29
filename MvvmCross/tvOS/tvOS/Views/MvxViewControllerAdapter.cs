// MvxViewControllerAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Core.Views;
using MvvmCross.Platform.tvOS.Views;

namespace MvvmCross.tvOS.Views
{
    public class MvxViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        public MvxViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxTvosView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxTvosView");
        }

        protected IMvxTvosView TvosView => ViewController as IMvxTvosView;

        public override void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
            TvosView.OnViewCreate();
            base.HandleViewDidLoadCalled(sender, e);
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            TvosView.OnViewDestroy();
            base.HandleDisposeCalled(sender, e);
        }
    }
}