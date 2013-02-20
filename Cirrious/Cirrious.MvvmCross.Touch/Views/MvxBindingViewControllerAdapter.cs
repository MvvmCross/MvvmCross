// MvxBindingTouchViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{

	public class MvxBindingViewControllerAdapter : MvxBaseViewControllerAdapter
	{
		protected IMvxBindingTouchView BindingTouchView {
			get {
				return ViewController as IMvxBindingTouchView;
			}
		}

        public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
			: base (eventSource)
		{
            if (!(eventSource is IMvxBindingTouchView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxBindingTouchView");
        }

		public override void HandleDisposeCalled (object sender, EventArgs e)
		{
            if (BindingTouchView == null)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "BindingTouchView is null for clearup of bindings in type {0}", BindingTouchView.GetType().Name);
                return;
            }
			BindingTouchView.ClearBindings();
			base.HandleDisposeCalled (sender, e);
		}
	}
}