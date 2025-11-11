// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using MvvmCross.Platforms.Ios.Views.Base;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Ios.Views
{
    public class MvxViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        protected IMvxIosView? IosView => ViewController as IMvxIosView;

        public MvxViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (eventSource is not IMvxIosView)
                throw new ArgumentException("eventSource should be a IMvxIosView", nameof(eventSource));
        }

        public override void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
            IosView?.OnViewCreate();
            base.HandleViewDidLoadCalled(sender, e);
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            IosView?.OnViewDestroy();
            base.HandleDisposeCalled(sender, e);
        }
    }
}
