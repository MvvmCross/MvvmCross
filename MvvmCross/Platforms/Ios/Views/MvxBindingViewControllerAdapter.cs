// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Logging;
using MvvmCross.Binding.BindingContext;
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
                throw new ArgumentException(nameof(eventSource), $"{nameof(eventSource)} should be a {nameof(IMvxIosView)}");

            IosView.BindingContext = Mvx.IoCProvider.Resolve<IMvxBindingContext>();
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            if (IosView == null)
            {
                MvxLog.Instance.Warn($"{nameof(IosView)} is null for clearup of bindings");
                return;
            }
            IosView.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}
