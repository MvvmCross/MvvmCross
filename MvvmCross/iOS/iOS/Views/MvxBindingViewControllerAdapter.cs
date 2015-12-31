// MvxBindingViewControllerAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Views
{
    using System;

    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.Touch.Views;

    public class MvxBindingViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        protected IMvxTouchView TouchView => this.ViewController as IMvxTouchView;

        public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxTouchView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxTouchView");

            this.TouchView.BindingContext = new MvxBindingContext();
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            if (this.TouchView == null)
            {
                MvxTrace.Warning("TouchView is null for clearup of bindings in type {0}",
                               this.TouchView?.GetType().Name);
                return;
            }
            this.TouchView.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}