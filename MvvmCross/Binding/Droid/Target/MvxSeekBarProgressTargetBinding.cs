// MvxSeekBarProgressTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Android.Widget;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxSeekBarProgressTargetBinding
        : MvxWithEventPropertyInfoTargetBinding<SeekBar>
    {
        public MvxSeekBarProgressTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        { 
            var progress = View;
            if (progress == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - progress is null in MvxSeekBarProgressTargetBinding");
            }
        }

        public override Type TargetType => typeof(int);
    }
}