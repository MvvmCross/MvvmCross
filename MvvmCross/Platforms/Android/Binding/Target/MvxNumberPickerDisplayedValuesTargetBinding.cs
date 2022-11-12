using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxNumberPickerDisplayedValuesTargetBinding : MvxTargetBinding<NumberPicker, IEnumerable<string>>
    {
        public MvxNumberPickerDisplayedValuesTargetBinding(NumberPicker target) : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValue(IEnumerable<string> value)
        {
            if (Target.MaxValue == 0)
                Target.MaxValue = value?.Count() - 1 ?? 0;
            Target.SetDisplayedValues(value?.ToArray());
            Target.Invalidate();
        }
    }
}
