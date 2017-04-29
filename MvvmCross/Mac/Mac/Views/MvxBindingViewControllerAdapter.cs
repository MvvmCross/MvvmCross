// MvxBindingViewControllerAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Mac.Views;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Mac.Views
{
    public class MvxBindingViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxMacView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxMacView");

            MacView.BindingContext = new MvxBindingContext();
        }

        protected IMvxMacView MacView => ViewController as IMvxMacView;

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            if (MacView == null)
            {
                MvxTrace.Warning("MacView is null for clearup of bindings in type {0}",
                    MacView.GetType().Name);
                return;
            }
            MacView.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}