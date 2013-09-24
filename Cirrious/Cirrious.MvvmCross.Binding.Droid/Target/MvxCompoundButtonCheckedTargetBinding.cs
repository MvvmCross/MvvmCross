// MvxCompoundButtonCheckedTargetBinding.cs
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
    public class MvxCompoundButtonCheckedTargetBinding
        : MvxPropertyInfoTargetBinding<CompoundButton>
    {
        private bool _subscribed;

        public MvxCompoundButtonCheckedTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        public override void SubscribeToEvents()
        {
            var compoundButton = View;
            if (compoundButton == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - compoundButton is null in MvxCompoundButtonCheckedTargetBinding");
                return;
            }

            _subscribed = true;
            compoundButton.CheckedChange += CompoundButtonOnCheckedChange;
        }

        private void CompoundButtonOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs args)
        {
            FireValueChanged(View.Checked);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var compoundButton = View;
                if (compoundButton != null && _subscribed)
                {
                    compoundButton.CheckedChange -= CompoundButtonOnCheckedChange;
                    _subscribed = false;
                }
            }
        }
    }
}