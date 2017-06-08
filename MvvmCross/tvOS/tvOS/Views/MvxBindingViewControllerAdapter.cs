// MvxBindingViewControllerAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;

namespace MvvmCross.tvOS.Views
{
    using System;

    using Binding.BindingContext;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.tvOS.Views;

    public class MvxBindingViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        protected IMvxTvosView TvosView => ViewController as IMvxTvosView;

        public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxTvosView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxTvosView");

            TvosView.BindingContext = Mvx.Resolve<IMvxBindingContext>();
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            if (TvosView == null)
            {
                MvxTrace.Warning("iosView is null for clearup of bindings in type {0}",
                               TvosView?.GetType().Name);
                return;
            }
            TvosView.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}