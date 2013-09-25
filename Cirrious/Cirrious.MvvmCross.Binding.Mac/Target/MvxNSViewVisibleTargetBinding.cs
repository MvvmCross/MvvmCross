// MvxNSViewVisibleTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoMac.AppKit;

namespace Cirrious.MvvmCross.Binding.Mac.Target
{
	public class MvxNSViewVisibleTargetBinding : MvxTargetBinding
	{
		protected NSView View
		{
			get { return (NSView) Target; }
		}

		public MvxNSViewVisibleTargetBinding(NSView target)
			: base(target)
		{
		}

		public override MvxBindingMode DefaultMode
		{
			get { return MvxBindingMode.OneWay; }
		}

		public override System.Type TargetType
		{
			get { return typeof (bool); }
		}

		public override void SetValue(object value)
		{
			var view = View;
			if (view == null)
				return;

			var visible = (bool) value;
			view.Hidden = !visible;
		}
	}
}