// MvxBindingViewControllerAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;

namespace MvvmCross.iOS.Views
{
    using System;

    using Binding.BindingContext;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.iOS.Views;

    public class MvxBindingViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        protected IMvxIosView IosView => ViewController as IMvxIosView;

        public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxIosView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxIosView");

            IosView.BindingContext = Mvx.Resolve<IMvxBindingContext>();
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            if (IosView == null)
            {
                MvxTrace.Warning("iosView is null for clearup of bindings in type {0}",
                               IosView?.GetType().Name);
                return;
            }
            IosView.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}