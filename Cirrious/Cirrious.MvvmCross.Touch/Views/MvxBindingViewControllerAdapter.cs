// MvxBindingTouchViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
	public class MvxBindingViewControllerAdapter : BaseViewControllerAdapter
	{
		protected IMvxBindingTouchView BindingTouchView {
			get {
				return TouchView as IMvxBindingTouchView;
			}
		}

        public MvxBindingViewControllerAdapter(IViewControllerEventSource view)
			: base (view)
		{			
		}

		public override void HandleIsDisposingDisposeCalled (object sender, EventArgs e)
		{
            if (BindingTouchView == null)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "BindingTouchView is null for clearup of bindings in type {0}", TouchView.GetType().Name);
                return;
            }
			BindingTouchView.ClearBindings();
			base.HandleIsDisposingDisposeCalled (sender, e);
		}
	}
}