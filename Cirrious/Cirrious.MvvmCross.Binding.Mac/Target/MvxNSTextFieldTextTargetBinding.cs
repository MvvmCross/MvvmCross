// MvxUITextFieldTextTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using System;

#if __UNIFIED__
using AppKit;
using Foundation;
#else
using MonoMac.AppKit;
using MonoMac.Foundation;
#endif


namespace Cirrious.MvvmCross.Binding.Mac.Target
{
    public class MvxNSTextFieldTextTargetBinding : MvxPropertyInfoTargetBinding<NSTextField>
    {
		public MvxNSTextFieldTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var editText = View;
            if (editText == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - NSTextField is null in MvxNSTextFieldTextTargetBinding");
            }
            else
            {
				editText.Changed += HandleEditTextChanged;
            }
        }

		private void HandleEditTextChanged(object sender, EventArgs e) {
			var view = View;
			if (view == null)
				return;
			FireValueChanged(view.StringValue);
		}

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

		protected override void SetValueImpl (object target, object value)
		{
			base.SetValueImpl (target, value ?? "");
		}

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var editText = View;
                if (editText != null)
                {
					editText.Changed -= HandleEditTextChanged;
                }
            }
        }
    }
}