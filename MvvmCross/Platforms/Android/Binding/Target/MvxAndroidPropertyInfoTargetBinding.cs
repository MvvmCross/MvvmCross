// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public abstract class MvxAndroidPropertyInfoTargetBinding : MvxPropertyInfoTargetBinding
    {
        protected MvxAndroidPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object value)
        {
            return MvxAndroidTargetBinding.TargetIsInvalid(target);
        }
    }

    public abstract class MvxAndroidPropertyInfoTargetBinding<TView>
        : MvxPropertyInfoTargetBinding<TView> where TView : class
    {
        protected MvxAndroidPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object value)
        {
            return MvxAndroidTargetBinding.TargetIsInvalid(target);
        }
    }
}
