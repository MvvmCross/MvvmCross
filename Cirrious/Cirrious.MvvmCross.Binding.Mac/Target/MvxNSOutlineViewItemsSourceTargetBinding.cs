using System;
using System.Linq;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoMac.AppKit;
using MonoMac.Foundation;
using System.Collections;
using System.Collections.Generic;
using Loqu8.KVC.Mac;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Mac
{
	// https://developer.apple.com/library/mac/documentation/cocoa/Conceptual/CollectionViews/Introduction/Introduction.html

	public class MvxNSOutlineViewItemsSourceTargetBinding : MvxConvertingTargetBinding
	{
		protected NSOutlineView View
		{
			get { return base.Target as NSOutlineView; }
		}

		public MvxNSOutlineViewItemsSourceTargetBinding(object target)
			: base(target)
		{
			var outlineView = View;
			if (outlineView == null)
			{
				MvxBindingTrace.Trace(MvxTraceLevel.Error,
					"Error - NSOutlineView is null in MvxNSOutlineViewItemSourceTargetBinding");
			}
			else
			{
			}
		}

		public override Type TargetType {
			get {
				return typeof(NSOutlineView);
			}
		}

		protected override void SetValueImpl (object target, object value)
		{
			// value should be an IEnumerable which we will wrap
			if (value is IEnumerable) {
				var dataSource = new NSOutlineViewDataSource ();
				View.DataSource = dataSource;
			}
		}

		public override MvxBindingMode DefaultMode
		{
			get { return MvxBindingMode.OneWay; }
		}

		protected override void Dispose(bool isDisposing)
		{
			base.Dispose(isDisposing);
			if (isDisposing)
			{
				var outlineView = View;
				if (outlineView != null)
				{
					// release
				}
			}
		}
	}
}


