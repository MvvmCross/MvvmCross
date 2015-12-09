// MvxViewControllerAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Mac.Views
{
    using System;

    using Cirrious.CrossCore.Mac.Views;

    using global::MvvmCross.Core.Views;

    public class MvxViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        protected IMvxMacView MacView
        {
            get { return base.ViewController as IMvxMacView; }
        }

        public MvxViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxMacView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxMacView");
        }

        public override void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
            this.MacView.OnViewCreate();
            base.HandleViewDidLoadCalled(sender, e);
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            this.MacView.OnViewDestroy();
            base.HandleDisposeCalled(sender, e);
        }
    }
}