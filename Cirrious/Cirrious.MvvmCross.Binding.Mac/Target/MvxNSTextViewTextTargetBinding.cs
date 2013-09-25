// MvxNSTextViewTextTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace Cirrious.MvvmCross.Binding.Mac.Target
{
    public class MvxNSTextViewTextTargetBinding : MvxNoFeedbackTargetBinding
    {
		protected NSTextView View
		{
			get { return base.Target as NSTextView; }
		}

		public MvxNSTextViewTextTargetBinding(object target)
            : base(target)
        {
            var editText = View;
            if (editText == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - NSTextView is null in MvxNSTextViewTextTargetBinding");
            }
            else
            {
				editText.TextDidChange += HandleTextDidChange;
			}
        }
	
		void HandleTextDidChange (object sender, EventArgs e)
		{
            var view = View;
            if (view == null)
                return;
			var str = View.TextStorage.Value;
            FireValueChanged(str);
        }

		public override Type TargetType {
			get {
				return typeof(NSTextView);
			}
		}

		protected override void TargetSetValue (object value)
		{
			View.TextStorage.SetString(new NSAttributedString(value as string));
		}

		public override void SetValue (object value)
		{
			MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Receiving setValue to " + (value ?? ""));
			var target = Target;
			if (target == null)
			{
				MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Weak Target is null in {0} - skipping set", GetType().Name);
				return;
			}

			base.SetValue (value);
		}

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var editText = View;
                if (editText != null)
                {
					editText.TextDidChange -= HandleTextDidChange;
                }
            }
        }
    }
}