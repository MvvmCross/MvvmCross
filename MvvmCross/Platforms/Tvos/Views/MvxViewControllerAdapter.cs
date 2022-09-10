// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Platforms.Tvos.Views.Base;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Tvos.Views
{
    public class MvxViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        protected IMvxTvosView TvosView => ViewController as IMvxTvosView;

        public MvxViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxTvosView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxTvosView");
        }

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
