// MvxNSViewVisibilityTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Contributed by Tim Uy, tim@loqu8.com

using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.UI;
using Cirrious.MvvmCross.Binding.Bindings.Target;

#if __UNIFIED__
using AppKit;
#else
using MonoMac.AppKit;
#endif

namespace Cirrious.MvvmCross.Binding.Mac.Target
{
	public class MvxNSViewVisibilityTargetBinding : MvxMacTargetBinding
	{
		protected NSView View
		{
			get { return (NSView) Target; }
		}

		public MvxNSViewVisibilityTargetBinding(NSView target)
			: base(target)
		{
		}

		public override MvxBindingMode DefaultMode
		{
			get { return MvxBindingMode.OneWay; }
		}

		public override System.Type TargetType
		{
			get { return typeof (MvxVisibility); }
		}

		protected override void SetValueImpl(object target, object value)
		{
			var view = View;
			if (view == null)
				return;

			var visibility = (MvxVisibility) value;
			switch (visibility)
			{
				case MvxVisibility.Visible:
					view.Hidden = false;
					break;
				case MvxVisibility.Collapsed:
					view.Hidden = true;
					break;
				default:
					MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Visibility out of range {0}", value);
					break;
			}
		}
	}
}