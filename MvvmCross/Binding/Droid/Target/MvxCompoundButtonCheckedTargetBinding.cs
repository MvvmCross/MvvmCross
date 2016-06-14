// MvxCompoundButtonCheckedTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;
using Android.Widget;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxCompoundButtonCheckedTargetBinding
        : MvxWithEventPropertyInfoTargetBinding<CompoundButton>
    {
        private bool _subscribed;

        public MvxCompoundButtonCheckedTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            EventSuffix = "Change";

            var button = View;
            if (button == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - button is null in MvxCompoundButtonCheckedTargetBinding");
            }
        }
    }
}