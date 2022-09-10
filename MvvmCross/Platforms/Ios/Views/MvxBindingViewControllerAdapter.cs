// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Logging;
using MvvmCross.Platforms.Ios.Views.Base;

namespace MvvmCross.Platforms.Ios.Views
{
    public class MvxBindingViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        protected IMvxIosView IosView => ViewController as IMvxIosView;

        public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxIosView))
                throw new ArgumentException($"{nameof(eventSource)} should be a {nameof(IMvxIosView)}", nameof(eventSource));

            if (Mvx.IoCProvider.TryResolve<IMvxBindingContext>(out var bindingContext))
                IosView.BindingContext = bindingContext;
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            if (IosView == null)
            {
                MvxLogHost.GetLog<MvxBindingViewControllerAdapter>()?.LogWarning(
                    "{iosView} is null for clearup of bindings", nameof(IosView));
                return;
            }
            IosView.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}
