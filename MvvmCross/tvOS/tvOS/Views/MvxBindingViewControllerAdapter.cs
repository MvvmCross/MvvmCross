// MvxBindingViewControllerAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;

namespace MvvmCross.iOS.Views
{
    using System;

    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.iOS.Views;

    public class MvxBindingViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        protected IMvxIosView IosView => this.ViewController as IMvxIosView;

        public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxIosView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxIosView");

            this.IosView.BindingContext = Mvx.Resolve<IMvxBindingContext>();
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            if (this.IosView == null)
            {
                MvxTrace.Warning("iosView is null for clearup of bindings in type {0}",
                               this.IosView?.GetType().Name);
                return;
            }
            this.IosView.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}