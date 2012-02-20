using System.Reflection;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Android.Target
{
    public class MvxCompoundButtonCheckedTargetBinding 
        : MvxPropertyInfoTargetBinding<CompoundButton>
    {
        public MvxCompoundButtonCheckedTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var compoundButton = View;
            if (compoundButton == null)
            {
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Error,
                                      "Error - compoundButton is null in MvxCompoundButtonCheckedTargetBinding");
            }
            else
            {
                compoundButton.CheckedChange += CompoundButtonOnCheckedChange;
            }
        }

        private void CompoundButtonOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs args)
        {
            FireValueChanged(View.Checked);
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
                var compoundButton = View;
                if (compoundButton != null)
                {
                    compoundButton.CheckedChange -= CompoundButtonOnCheckedChange;
                }
            }
        }
    }
}