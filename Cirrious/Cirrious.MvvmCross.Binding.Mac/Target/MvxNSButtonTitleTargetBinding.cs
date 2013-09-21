// MvxUIButtonTitleTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoMac.AppKit;

namespace Cirrious.MvvmCross.Binding.Mac.Target
{
    public class MvxNSButtonTitleTargetBinding : MvxTargetBinding
    {
        protected NSButton Button
        {
            get { return base.Target as NSButton; }
        }

        public MvxNSButtonTitleTargetBinding(NSButton button)
            : base(button)
        {
            if (button == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - NSButton is null in MvxNSButtonTitleTargetBinding");
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override System.Type TargetType
        {
            get { return typeof (string); }
        }

        public override void SetValue(object value)
        {
            var button = Button;
            if (button == null)
                return;
		
			button.Title = value as string;
        }
    }
}