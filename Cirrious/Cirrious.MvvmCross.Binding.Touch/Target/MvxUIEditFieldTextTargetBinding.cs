// MvxUIEditFieldTextTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUITextFieldTextTargetBinding : MvxPropertyInfoTargetBinding<UITextField>
    {
        public MvxUITextFieldTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var editText = View;
            if (editText == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - UITextField is null in MvxUITextFieldTextTargetBinding");
            }
            else
            {
                editText.EditingChanged += HandleEditTextValueChanged;
            }
        }

        private void HandleEditTextValueChanged(object sender, System.EventArgs e)
        {
            FireValueChanged(View.Text);
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
                    editText.EditingChanged -= HandleEditTextValueChanged;
                }
            }
        }
    }
}