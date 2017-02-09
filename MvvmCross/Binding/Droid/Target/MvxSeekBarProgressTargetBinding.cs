// MvxSeekBarProgressTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;
using Android.Widget;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using System;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxSeekBarProgressTargetBinding
        : MvxPropertyInfoTargetBinding<SeekBar>
    {
        public MvxSeekBarProgressTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        private IDisposable _subscription;

        // this variable isn't used, but including this here prevents Mono from optimising the call out!
        private int JustForReflection
        {
            get { return View.Progress; }
            set { View.Progress = value; }
        }

        protected override void SetValueImpl(object target, object value)
        {
            var seekbar = (SeekBar)target;
            if (seekbar == null)
                return;

            seekbar.Progress = (int)value;
        }

        private void SeekBarProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (e.FromUser)
                FireValueChanged(e.Progress);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var seekBar = View;
            if (seekBar == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - SeekBar is null in MvxSeekBarProgressTargetBinding");
                return;
            }

            _subscription = seekBar.WeakSubscribe<SeekBar, SeekBar.ProgressChangedEventArgs>(
                nameof(seekBar.ProgressChanged),
                SeekBarProgressChanged);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }
            base.Dispose(isDisposing);
        }
    }
}