// MvxAndroidTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Android.Widget;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxRatingBarRatingTargetBinding 
        : MvxWithEventPropertyInfoTargetBinding<RatingBar>
    {
        
        public MvxRatingBarRatingTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            EventSuffix = "BarChage";
        }

        public override Type TargetType => typeof(float);
    }
}