// MvxBindingViewControllerAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Interfaces;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxBindingViewControllerAdapter : MvxBaseViewControllerAdapter
    {
        protected IMvxTouchView TouchView
        {
            get { return ViewController as IMvxTouchView; }
        }

        public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxTouchView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxTouchView");

            TouchView.BindingContext = new MvxBindingContext();
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            if (TouchView == null)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "TouchView is null for clearup of bindings in type {0}",
                               TouchView.GetType().Name);
                return;
            }
            TouchView.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}