// MvxUITextViewTextTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUITextViewTextTargetBinding : MvxConvertingTargetBinding
    {
        private IDisposable _subscription;

        protected UITextView View => Target as UITextView;


        public MvxUITextViewTextTargetBinding(UITextView target)
            : base(target)
        {
        }

        private void EditTextOnChanged(object sender, NSTextStorageEventArgs eventArgs)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(view.Text);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var target = View;
            if (target == null)
            {
                MvxBindingTrace.Error(
                                      "Error - UITextView is null in MvxUITextViewTextTargetBinding");
                return;
            }

			var textStorage = target.LayoutManager?.TextStorage;
			if (textStorage == null)
			{ 
			    MvxBindingTrace.Error(
						  "Error - NSTextStorage of UITextView is null in MvxUITextViewTextTargetBinding");
				return;
			}

            _subscription = textStorage.WeakSubscribe<NSTextStorage, NSTextStorageEventArgs>(nameof(textStorage.DidProcessEditing), EditTextOnChanged);
        }

        public override Type TargetType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UITextView)target;
            if (view == null) return;

            view.Text = (string)value;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }
    }
}