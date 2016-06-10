// MvxAutoCompleteTextViewSelectedObjectTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;

using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxAutoCompleteTextViewSelectedObjectTargetBinding
        : MvxWithEventPropertyInfoTargetBinding<MvxAutoCompleteTextView>
    {
        public MvxAutoCompleteTextViewSelectedObjectTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var autoComplete = View;
            if (autoComplete == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - autoComplete is null in MvxAutoCompleteTextViewSelectedObjectTargetBinding");
            }
        }
    }
}