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

	public class MvxNSCollectionViewItemsSourceTargetBinding : MvxConvertingTargetBinding
	{
		protected NSCollectionView View
		{
			get { return base.Target as NSCollectionView; }
		}

		public MvxNSCollectionViewItemsSourceTargetBinding(object target)
			: base(target)
		{
			var collectionView = View;
			if (collectionView == null)
			{
				MvxBindingTrace.Trace(MvxTraceLevel.Error,
					"Error - NSCollectionView is null in MvxNSCollectionViewItemSourceTargetBinding");
			}
			else
			{
			}
		}

		public override Type TargetType {
			get {
				return typeof(NSCollectionView);
			}
		}

		protected override void SetValueImpl (object target, object value)
		{
			// value should be an IEnumerable which we will wrap
			if (value is IEnumerable) {
				var objs = new List<NSObject> ();
				var items = (IEnumerable)value;
				foreach (var item in items) {
					var wrapped = new KVCWrapper (item);		// recursive?
					objs.Add (wrapped);
				}
				View.Content = objs.ToArray();
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
				var collectionView = View;
				if (collectionView != null)
				{
					// release
				}
			}
		}
	}
}


