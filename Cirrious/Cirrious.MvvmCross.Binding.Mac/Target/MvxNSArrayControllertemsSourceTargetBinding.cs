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

	public class MvxNSArrayControllerItemsSourceTargetBinding : MvxConvertingTargetBinding
	{
		protected NSArrayController Controller
		{
			get { return base.Target as NSArrayController; }
		}

		public MvxNSArrayControllerItemsSourceTargetBinding(object target)
			: base(target)
		{
			var arrayController = Controller;
			if (arrayController == null)
			{
				MvxBindingTrace.Trace(MvxTraceLevel.Error,
					"Error - NSArrayController is null in MvxNSNSArrayControllerItemsSourceTargetBinding");
			}
			else
			{
			}
		}

		public override Type TargetType {
			get {
				return typeof(NSArrayController);
			}
		}

		protected override void SetValueImpl (object target, object value)
		{
			if (value is IEnumerable) {
				// clear content
				if (Controller.Content != null) {
					((NSMutableArray)(Controller.Content)).RemoveAllObjects ();		// what if it is not mutable...
				}	
				var objs = new List<NSObject> ();
				var items = (IEnumerable)value;
				foreach (var item in items) {
					var wrapped = new KVCWrapper (item);		// recursive?
					Controller.AddObject (wrapped);
				}
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
				var arrayController = Controller;
				if (arrayController != null)
				{
					// release
				}
			}
		}
	}
}


