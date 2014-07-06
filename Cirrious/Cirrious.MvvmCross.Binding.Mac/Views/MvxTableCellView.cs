// MvxView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using MonoMac.AppKit;
using MonoMac.Foundation;
using System.Collections;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using System.Collections.Specialized;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.CrossCore.Core;
using System.Linq;

namespace Cirrious.MvvmCross.Binding.Mac.Views
{
	[Register("MvxTableCellView")]
	public class MvxTableCellView : NSTableCellView, IMvxBindingContextOwner, IMvxDataConsumer
	{
		// Called when created from unmanaged code
		public MvxTableCellView (IntPtr handle) : base (handle)
		{
			Initialize (string.Empty);
		}

		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MvxTableCellView (NSCoder coder) : base (coder)
		{
			Initialize (string.Empty);
		}

		public MvxTableCellView(string bindingText)
		{
			this.Frame = new RectangleF (0, 0, 100, 17);
			TextField = new NSTextField (new RectangleF(0,0,100,17))
			{
				Editable = false,
				Bordered = false,
				BackgroundColor = NSColor.Clear,
			};

			AddSubview (TextField);
			Initialize (bindingText);
		}

		public override RectangleF Frame {
			get {
				return base.Frame;
			}
			set {
				base.Frame = value;
				if (TextField != null)
					TextField.Frame = value;
			}
		}

		public event EventHandler Click {
			add {
				TextField.Activated += value;
			}
			remove {
				TextField.Activated -= value;
			}
		}

		// Shared initialization code
		void Initialize (string bindingText)
		{
			this.CreateBindingContext(bindingText);
		}

		public IMvxBindingContext BindingContext {
			get;
			set;
		}

		public object DataContext
		{
			get { return BindingContext.DataContext; }
			set { BindingContext.DataContext = value; }
		}

		public string Text {
			get{
				return this.TextField.StringValue;
			}
			set{
				this.TextField.StringValue = value;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				BindingContext.ClearAllBindings();
			}
			base.Dispose(disposing);
		}
	}	
}