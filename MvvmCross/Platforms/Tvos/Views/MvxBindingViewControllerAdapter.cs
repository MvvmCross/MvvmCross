// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Logging;
using MvvmCross.Platforms.Tvos.Views.Base;

namespace MvvmCross.Platforms.Tvos.Views
{
    public class MvxBindingViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        protected IMvxTvosView TvosView => ViewController as IMvxTvosView;

        public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxTvosView))
                throw new ArgumentException(nameof(eventSource), $"{nameof(eventSource)} should be a {nameof(IMvxTvosView)}");

            TvosView.BindingContext = Mvx.IoCProvider.Resolve<IMvxBindingContext>();
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            if (TvosView == null)
            {
                MvxLogHost.GetLog<MvxBindingViewControllerAdapter>()?.Log(
                    LogLevel.Warning, "{viewName} is null for clearup of bindings", nameof(TvosView));
                return;
            }
            TvosView.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}
