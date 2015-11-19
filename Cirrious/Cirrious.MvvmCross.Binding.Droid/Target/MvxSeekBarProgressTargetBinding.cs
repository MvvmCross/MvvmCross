// MvxSeekBarProgressTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;
using Android.Widget;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxSeekBarProgressTargetBinding 
        : MvxPropertyInfoTargetBinding<SeekBar>
    {
		public MvxSeekBarProgressTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        private bool _subscribed;

        // this variable isn't used, but including this here prevents Mono from optimising the call out!
        private int JustForReflection
        {
            get { return View.Progress; }
            set { View.Progress = value; }
        }

        protected override void SetValueImpl(object target, object value)
        {
            var seekbar = (SeekBar) target;
            if (seekbar == null)
                return;

            seekbar.Progress = (int) value;
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

            seekBar.ProgressChanged += SeekBarProgressChanged;
            _subscribed = true;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var view = View;
                if (view != null && _subscribed)
                {
                    view.ProgressChanged -= SeekBarProgressChanged;
                    _subscribed = false;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}