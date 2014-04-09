using System;
using System.Linq;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoMac.AppKit;
using MonoMac.Foundation;
using System.Collections;
using System.Collections.Generic;
using Loqu8.KVC.Mac;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore;

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
				Mvx.Trace ("AC: Removing array controller content");

				if (Controller.Content != null) {
					((NSMutableArray)(Controller.Content)).RemoveAllObjects ();		// what if it is not mutable...
				}	

				var items = (IEnumerable)value;
				var objs = new List<NSObject> ();
				foreach (var item in items) {
					var wrapped = new KVCWrapper (item);		// recursive?
					objs.Add (wrapped);
				}

				Mvx.Trace ("AC: Wrapped {0} objects", objs.Count);
				var toarray = objs.ToArray ();
				Mvx.Trace ("AC: Converted to NSObject[]");
				var nsarray = NSArray.FromNSObjects(objs.ToArray ());
				Mvx.Trace ("AC: Converted NSObject[] to NSArray");
				Controller.AddObjects (nsarray);
				Mvx.Trace ("AC: Added objects to Controller");
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


