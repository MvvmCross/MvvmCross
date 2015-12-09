// MvxBindingViewControllerAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Mac.Views
{
    using System;

    using Cirrious.CrossCore.Mac.Views;

    using global::MvvmCross.Binding.BindingContext;
    using global::MvvmCross.Platform.Platform;

    public class MvxBindingViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        protected IMvxMacView MacView
        {
            get { return this.ViewController as IMvxMacView; }
        }

        public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxMacView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxMacView");

            this.MacView.BindingContext = new MvxBindingContext();
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            if (this.MacView == null)
            {
                MvxTrace.Warning("MacView is null for clearup of bindings in type {0}",
                               this.MacView.GetType().Name);
                return;
            }
            this.MacView.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}